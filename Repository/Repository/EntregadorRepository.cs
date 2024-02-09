using Dapper;
using Domain.Commands;
using Domain.Interface;
using Domain.ViewModel;
using Npgsql;
using System.Data;
using RabbitMQ.Client;


namespace Repository.Repository
{
    public class EntregadorRepository : IEntregadorRepository
    {

        string conexao = @"Host=localhost;Port=5432;Username=postgres;Password=*****;Database=AluguelVeiculos";
        public async Task<string> PostEntregadorAsync(EntregadorCommand command)
        {
            string queryInsert = @"
            INSERT INTO CadastroEntregador (nomeEntregador,cnpjEntregador,nascimentoEntregador,numeroCNH,tipoCNH) 
            VALUES (@Nome,@cnpj,@Nascimento,@Habilitacao,@TipoCNH)";

            using (IDbConnection conn = new NpgsqlConnection(conexao))
            {
                conn.Execute(queryInsert, new
                {
                    Nome = command.nomeEntregador,
                    cnpj = command.cnpjEntregador,
                    Nascimento = command.nascimentoEntregador,
                    Habilitacao = command.numeroCNH,
                    TipoCNH = command.tipoCNH
                });
            }

            return "Cliente cadastrado com sucesso!";

        }

        public async Task<AlugarVeiculoViewModel> PostAlugarAsync(string cnpj, AlugarVeiculoViewModel aluguelViewModel)
        {   
            string queryAlugar = "UPDATE CadastroVeiculo SET fk_entregadorid = @entregadorID, alugado=True WHERE veiculoID = @veiculoID ";
            string queryUpdateEntregador = "UPDATE CadastroEntregador SET locacao = True WHERE entregadorID = @entregadorID";

            int entregadorID = await GetEntregadorID(cnpj);
            int veiculoID = await GetVeiculosDisponiveis();
            using (IDbConnection conn = new NpgsqlConnection(conexao))
            {


                conn.Execute(queryAlugar, new
                {
                    entregadorID = entregadorID,
                    veiculoID = veiculoID
                });

                conn.Execute(queryUpdateEntregador, new
                {
                    entregadorID = entregadorID
                });
                var alugar = await SimularAluguel(aluguelViewModel.plano, cnpj, aluguelViewModel.dataInicio, aluguelViewModel.dataDevolucao);
                
               return alugar;
            }
        }

        public async Task<string> PutImageAsync(string cnpj, ImagemCommand imagemCommand)
        {             
            string queryUploadImg = @"UPDATE CadastroEntregador SET arquivoCNH=@nomeArquivo WHERE cnpjEntregador=@cnpj";
            string caminhoCompleto = imagemCommand.nomeArquivo;

            string nomeArquivo = Path.GetFileName(caminhoCompleto);

            using (IDbConnection conn = new NpgsqlConnection(conexao))
            {

                conn.Execute(queryUploadImg, new
                {
                    cnpj = cnpj,
                    nomeArquivo = nomeArquivo
                });

                var streamIMG = await RecuperarFotoCNHAsync(imagemCommand.nomeArquivo);

                await SalvarFotoCNHAsync(nomeArquivo, streamIMG);

                return "Imagem Cadastrada com sucesso!";

            }
        }

        public async Task<string> SalvarFotoCNHAsync(string nomeArquivo, Stream fotoCNH)
        {
            try
            {
                string caminho = @"C:\Users\Usuário\source\repos\AluguelVeiculo\Repository\Images\";

                if (!Directory.Exists(caminho))
                {
                    Directory.CreateDirectory(caminho);
                }

                string nomeArquivoFotoCNH = $"{nomeArquivo}.png";
                string caminhoCompletoFotoCNH = Path.Combine(caminho, nomeArquivoFotoCNH);

                using (var fileStream = new FileStream(caminhoCompletoFotoCNH, FileMode.Create))
                {
                    await fotoCNH.CopyToAsync(fileStream);
                }

                return "Imagem salva com sucesso";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar foto da CNH: {ex.Message}");
                return "Não foi possível salvar a imagem!";
            }
        }

        public async Task<Stream> RecuperarFotoCNHAsync(string nomeArquivo)
        {

            return new FileStream(nomeArquivo, FileMode.Open);
        }

        public async Task<bool> ValidaCNPJ(string cnpj)
        {
           string queryValidaCnpj = @"SELECT cnpjEntregador FROM CadastroEntregador WHERE cnpjEntregador=@cnpj";

            using (IDbConnection conn = new NpgsqlConnection(conexao))
            {

                string cnpjCadastrado = await conn.QueryFirstOrDefaultAsync<string>(queryValidaCnpj, new { cnpj = cnpj });

                if (cnpjCadastrado == cnpj && cnpjCadastrado != null)
                {
                    return true;
                }
                
               return false;
                
            }            
        }
        public async Task<string> ValidaCNH(string cnh)
        {   
           string queryValidaCnh = @"SELECT tipoCNH FROM CadastroEntregador WHERE numerocnh=@cnh";

            using (IDbConnection conn = new NpgsqlConnection(conexao))
            {

                string categoriaCNH  = await conn.QueryFirstOrDefaultAsync<string>(queryValidaCnh, new { cnh = cnh });

                if (categoriaCNH != null)
                {
                    return categoriaCNH;
                }

                return null;
            }            
        }       

        public async Task<bool> VerificaDataMaior(DateTime dataInicio, DateTime dataDevolucao)
        {
            int result = DateTime.Compare(dataInicio, dataDevolucao);

            if(result > 0)
            {
                return true;
            }

            return false;
        }

        public async Task<decimal> GetValorDiaria(int plano)
        {
            string queryGetDiaria = @"SELECT valordiaria FROM planoslocacao WHERE dias=@plano";

            using (IDbConnection conn = new NpgsqlConnection(conexao))
            {
                decimal diaria = await conn.QueryFirstOrDefaultAsync<decimal>(queryGetDiaria, new { plano = plano });
                return diaria;                      
            }            
        }

        public async Task<int> GetVeiculosDisponiveis()
        {
            string queryGetDisponivel = @"SELECT veiculoID FROM CadastroVeiculo WHERE alugado=false";

            using (IDbConnection conn = new NpgsqlConnection(conexao))
            {
                int disponibilidade = await conn.QueryFirstOrDefaultAsync<int>(queryGetDisponivel);

                return disponibilidade;
            }
        }

        public async Task<AlugarVeiculoViewModel> SimularAluguel(int plano, string cnpj, DateTime dataInicio, DateTime dataDevolucao)
        {
            decimal taxaPlano7 = 0.2m;
            decimal taxaPlano15 = 0.4m;
            decimal taxaPlano30 = 0.6m;

            var simulacao = new AlugarVeiculoViewModel();
            var diaria = await GetValorDiaria(plano);

            simulacao.plano = plano;
            simulacao.dataInicio = dataInicio;
            simulacao.dataDevolucao = dataDevolucao;
            simulacao.valorDiaria = diaria;
            if(plano == 7)
            {
              simulacao.dataPrevisaoDevolucao = dataInicio.AddDays(7);
            }
            else if(plano == 15)
            {
                simulacao.dataPrevisaoDevolucao = dataInicio.AddDays(15);
            }          
            else if(plano == 30)
            {
              simulacao.dataPrevisaoDevolucao = dataInicio.AddDays(30);
            }        
                    
            int diferencaDias = (simulacao.dataPrevisaoDevolucao - dataDevolucao).Days;

            if(diferencaDias > 0 && simulacao.plano == 7)
            {
                simulacao.valorDiaria = (diaria + (diaria * taxaPlano7)+diaria)/2;
            }

            else if (diferencaDias > 0 && simulacao.plano == 15)
            {
                simulacao.valorDiaria = (diaria + (diaria * taxaPlano15) + diaria) / 2;
            }

            else if (diferencaDias > 0 && simulacao.plano == 30)
            {
                simulacao.valorDiaria = (diaria + (diaria * taxaPlano30) + diaria) / 2;
            }

            diferencaDias = (dataDevolucao - simulacao.dataPrevisaoDevolucao).Days;

            if (diferencaDias > 0)
            {
                decimal acrescimo = (50 * diferencaDias) + simulacao.valorTotal;
                simulacao.valorTotal = acrescimo;
            }

            simulacao.valorTotal = simulacao.valorDiaria * simulacao.plano;

            return simulacao;
        }

        public async Task<int> GetEntregadorID(string cnpj)
        {
            string queryGetID = @"SELECT entregadorID FROM CadastroEntregador WHERE cnpjEntregador=@cnpj";

            using (IDbConnection conn = new NpgsqlConnection(conexao))
            {
                int entregadorID = await conn.QueryFirstOrDefaultAsync<int>(queryGetID, new {cnpj = cnpj});
                return entregadorID;
            }
        }
    }
}
    
