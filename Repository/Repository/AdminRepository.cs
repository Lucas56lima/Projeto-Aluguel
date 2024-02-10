using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using Dapper;
using Domain.Commands;
using Domain.Interface;
using Npgsql;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Repository.Repository
{
    public class AdminRepository : IAdminRepository
    {
        string conexao = @"Host=localhost;Port=5432;Username=postgres;Password=123456;Database=AluguelVeiculos";

        public async Task<string> PostAsyncVeiculo(VeiculoCommand command)
        {
            string queryInsert = @"
            INSERT INTO CadastroVeiculo (dataVeiculo,modeloVeiculo,placaVeiculo,anoVeiculo) 
            VALUES (@Data,UPPER(@Modelo),UPPER(@Placa),@Ano)";

            using (IDbConnection conn = new NpgsqlConnection(conexao))
            {
                conn.Execute(queryInsert, new
                {
                    Data = command.dataVeiculo,
                    Modelo = command.modeloVeiculo,
                    Placa = command.placaVeiculo,
                    Ano = command.anoVeiculo,

                });
            }
            return "Veículo cadastrado com sucesso!";
        }

        public async Task<IEnumerable<VeiculoCommand>> GetAsyncVeiculo(string? placa)
        {
            string queryConsulta = "";
            if (placa == null || placa == "")
            {
                queryConsulta = @"SELECT * FROM CadastroVeiculo";
            }
            else
            {
                queryConsulta = @"SELECT * FROM CadastroVeiculo WHERE placaVeiculo = UPPER(@placa)";
            }

            using (IDbConnection conn = new NpgsqlConnection(conexao))
            {
                return await conn.QueryAsync<VeiculoCommand>(queryConsulta, new { placa = placa });

            }
        }
        public async Task<bool> VerificaPlaca(string placa)
        {   
            
            string queryGetPlaca = @"SELECT placaVeiculo FROM CadastroVeiculo WHERE placaVeiculo = @placa";

            using (IDbConnection conn = new NpgsqlConnection(conexao))
            {
                string placaVerificada = conn.Query<string>(queryGetPlaca, new { placa = placa }).FirstOrDefault();

                if (placa == placaVerificada)
                {
                    return true;
                }
                return false;
            }
        }
        public async Task<string> PutAsyncVeiculo(int veiculoID, string placa)
        {
            string queryUpdate = @"
            UPDATE CadastroVeiculo SET placaVeiculo=UPPER(@placa) WHERE veiculoID=@veiculoID";

            using (IDbConnection conn = new NpgsqlConnection(conexao))
            {
                conn.Execute(queryUpdate, new { veiculoID = veiculoID, placa = placa });
            }

            return "Placa atualizada com sucesso!";
        }

        public async Task<string> DeleteAsyncVeiculo(int veiculoID)
        {
            string queryInativate = @"
            DELETE FROM CadastroVeiculo WHERE veiculoID=@veiculoID AND alugado=False";

            using (IDbConnection conn = new NpgsqlConnection(conexao))
            {
                conn.Execute(queryInativate, new { veiculoID = veiculoID });
            }

            return "Cadastro excluído com sucesso!";
        }
        public async Task<string> PostAsyncPedido(PedidoCommand pedidoCommand)
        {               
            string queryInsert = @"
            INSERT INTO Pedidos (dataCriacao,valorCorrida) 
            VALUES (@Data,@ValorCorrida)";

            using (IDbConnection conn = new NpgsqlConnection(conexao))
            {
                conn.Execute(queryInsert, new
                {
                    Data = pedidoCommand.dataCriacao,
                    ValorCorrida = pedidoCommand.valorCorrida,                    
                });
            }

            await enviarNotificacao(pedidoCommand.pedidoID, pedidoCommand.valorCorrida);
            await RecebeResposta();
            return "Pedido cadastrado com sucesso!";
        }
        public async Task enviarNotificacao(int pedidoID,decimal valorCorrida)
        {        
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ConfirmSelect();
                channel.BasicAcks += Channel_BasicAcks;
                channel.BasicNacks += Channel_BasicNacks;

                channel.QueueDeclare(queue: "fila",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

              string mensagem = $@"              
              Assunto: Novo pedido disponível
              Pedido: {pedidoID}              
              Valor da Corrida: {valorCorrida}

              Olá,

              Há um pedido disponível para entrega, gostaria de confirmar?
              Responda com 'sim' ou 'não'.
              Obrigado!
              ";
                string mensagemFormatada = Regex.Replace(mensagem, @"\s+", " ");
                mensagemFormatada = mensagemFormatada.TrimStart();
                mensagemFormatada = mensagemFormatada.TrimEnd();

                SalvaMensagemNoBanco(mensagemFormatada, pedidoID);                   
              var entregadores = await GetEntregadoresDisponiveis();

                foreach (var entregador in entregadores)
                {
                    mensagem = $@"{mensagem} 
                    ID Entregador:{entregador.entregadorID}";
                    var body = Encoding.UTF8.GetBytes(mensagem);

                    channel.BasicPublish(exchange:  "",
                                                routingKey: "fila_entregadores",
                                                basicProperties: null,
                                                body: body);
                    Console.WriteLine($"Mensagem enviada para o entregador {entregador}");
                }              
            }           
        }
        private void Channel_BasicNacks(object? sender, BasicNackEventArgs e)
        {
            Console.WriteLine("Nack");
        }

        private void Channel_BasicAcks(object? sender, BasicAckEventArgs e)
        {
            Console.WriteLine("ack");
        }

        public async Task RecebeResposta()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "fila",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                
                bool respostaSimRecebida = false;                

                try
                {
                    while (!respostaSimRecebida)
                    {
                        var data = await Task.Run(() => channel.BasicGet("fila", true));
                        if (data == null) // Nenhuma mensagem disponível
                        {
                            break;
                        }

                        var mensagem = Encoding.UTF8.GetString(data.Body.ToArray());
                        var entregadorID = ExtrairIdEntregadorDaMensagem(mensagem);
                        var pedidoID = ExtrairIdPedido(mensagem);

                        Console.WriteLine($"Resposta recebida do entregador ID {entregadorID}: {mensagem}");
                        
                        if (mensagem.ToLower().Contains("sim"))
                        {
                            respostaSimRecebida = true;

                            // Atualiza as tabelas com os dados recebidos
                            AtualizarBancoDeDados(mensagem, entregadorID, pedidoID);
                        }
                        else
                        {
                            Console.WriteLine("Nenhuma mensagem recebida");
                        }

                        await Task.Delay(1000);
                        

                    }
                } catch (Exception ex)
                {
                    Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                }                
            }
        }

        private void AtualizarBancoDeDados(string mensagem, int entregadorID, int pedidoID)
        {
            // Atualiza a tabela Mensagens
            string queryUpdateMensagem = @"
            UPDATE Mensagens SET entregadorID_resposta=@entregadorID, resposta=@mensagem WHERE pedidoID = @pedidoID";

            using (IDbConnection conn = new NpgsqlConnection(conexao))
            {
                conn.Execute(queryUpdateMensagem, new
                {
                    entregadorID = entregadorID,
                    mensagem = mensagem,
                    pedidoID = pedidoID
                });
            }
            
            string queryUpdatePedido = @"
            UPDATE Pedidos SET fk_entregadorID=@entregadorID WHERE pedidoID = @pedidoID";

            using (IDbConnection conn = new NpgsqlConnection(conexao))
            {
                conn.Execute(queryUpdatePedido, new
                {
                    entregadorID = entregadorID,
                    pedidoID = pedidoID
                });
            }
        }

        private void SalvaMensagemNoBanco(string mensagem,int pedidoID)
        {
            string queryInsert = @"
                INSERT INTO Mensagens (dataMensagem,pedidoID,mensagem) 
                VALUES (@Data,@PedidoID,@Mensagem)";

            using (IDbConnection conn = new NpgsqlConnection(conexao))
            {
                conn.Execute(queryInsert, new
                {
                    Data = DateTime.Now,
                    PedidoID = pedidoID,
                    Mensagem = mensagem
                });
            }
        }

        int ExtrairIdEntregadorDaMensagem(string mensagem)
            {
                var partes = mensagem.Split(':');
                if (partes.Length >= 4 && int.TryParse(partes[3], out int id))
                {
                    return id;
                }
                return -1;
            }

            int ExtrairIdPedido(string mensagem)
            {
                var partes = mensagem.Split(':');
                if (partes.Length >= 2 && int.TryParse(partes[1], out int id))
                {
                    return id;
                }
                return -1;
            }       

        public async Task<IEnumerable<EntregadorCommand>> GetEntregadoresDisponiveis() 
        {
            string conexao = @"Host=localhost;Port=5432;Username=postgres;Password=15975323;Database=AluguelVeiculos";
            string queryGetEntregadores = @"SELECT CadastroEntregador.entregadorID,Pedidos.fk_entregadorID FROM CadastroEntregador
            LEFT JOIN Pedidos ON CadastroEntregador.entregadorID = Pedidos.fk_entregadorID WHERE CadastroEntregador.locacao=True AND Pedidos.fk_entregadorid IS NULL            
            ";            
            using (IDbConnection conn = new NpgsqlConnection(conexao))
            {
                var entregadores = await conn.QueryAsync<EntregadorCommand>(queryGetEntregadores);

                return entregadores;
            }
        }        
        
    }    
}
    

