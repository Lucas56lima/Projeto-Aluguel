using System.Data;
using Dapper;
using Domain.Commands;
using Domain.Interface;
using Npgsql;

namespace Repository.Repository
{      
    public class EntregadorRepository : IEntregadorRepository
    {
       
        string conexao = @"Host=localhost;Port=5432;Username=postgres;Password=15975323;Database=Veiculo";
        public async Task<string> PostAsync(EntregadorCommand command)
        {
            string queryInsert = @"
            INSERT INTO Clientes (nomeEntregador,cnpjEntregador,nascimentoEntregador,numeroCNH,tipoCNH) 
            VALUES (@Nome,@cnpj,@Nascimento,@Habilitacao,@TipoCNH)";

            using (IDbConnection conn = new NpgsqlConnection(conexao))
            {
                var fotoCNH = new MemoryStream();
                conn.Execute(queryInsert, new
                {
                    Nome = command.nomeEntregador,
                    Cpf = command.cnpjEntregador,
                    Nascimento = command.nascimentoEntregador,
                    Habilitacao = command.numeroCNH,
                    TipoCNH = command.tipoCNH,
                    FotoCNH = fotoCNH,
                }); 
            }
            
            return "Cliente cadastrado com sucesso!";

        }

        public async Task<bool> SalvarFotoCNH(string caminho, Stream fotoCNH)
        {
            try
            {
                // Verifica se a pasta para armazenar as fotos da CNH existe, se não, cria-a
                if (!Directory.Exists(caminho))
                {
                    Directory.CreateDirectory(caminho);
                }
                // Gera um nome de arquivo único para a foto da CNH
                string nomeArquivoFotoCNH = $"{Guid.NewGuid()}.png"; // Ou .bmp se preferir
                string caminhoCompletoFotoCNH = Path.Combine(caminho, nomeArquivoFotoCNH);

                // Salva a foto da CNH no disco local
                using (var fileStream = new FileStream(caminhoCompletoFotoCNH, FileMode.Create))
                {
                    await fotoCNH.CopyToAsync(fileStream);
                }

                return true; // Sucesso ao salvar a foto
            }
            catch (Exception ex)
            {
                // Tratamento de exceção
                Console.WriteLine($"Erro ao salvar foto da CNH: {ex.Message}");
                return false;
            }
        }

    }
}
    
