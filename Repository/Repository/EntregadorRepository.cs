using Dapper;
using Domain.Commands;
using Domain.Interface;
using Npgsql;
using System.Data;


namespace Repository.Repository
{
    public class EntregadorRepository : IEntregadorRepository
    {

        string conexao = @"Host=localhost;Port=5432;Username=postgres;Password=15975323;Database=AluguelVeiculos";
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

        public async Task<string> PutImageAsync(string cnpj, ImagemCommand imagemCommand)
        {
            string conexao = @"Host=localhost;Port=5432;Username=postgres;Password=15975323;Database=AluguelVeiculos";
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

        public async Task<int> ValidaCNPJ(string cnpj)
        {
            string conexao = @"Host=localhost;Port=5432;Username=postgres;Password=15975323;Database=AluguelVeiculos";
            string queryValidaCnpj = @"SELECT cnpjentregador FROM CadastroEntregador WHERE cnpjentregador=@cnpj";

            using (IDbConnection conn = new NpgsqlConnection(conexao))
            {

                int cnpjCadastrado = await conn.QueryFirstOrDefaultAsync<int>(queryValidaCnpj, new { cnpj = cnpj });

                if (cnpjCadastrado >= 0 && cnpjCadastrado != null)
                {
                    return cnpjCadastrado;
                }
                else
                {
                    return -1;
                }
            }            
        }
        public async Task<string> ValidaCNH(string cnh)
        {

            string conexao = @"Host=localhost;Port=5432;Username=postgres;Password=15975323;Database=AluguelVeiculos";
            string queryValidaCnh = @"SELECT categoria FROM CadastroEntregador WHERE numerocnh=@cnh";

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
        public async Task<string> PostAlugarAsync(int plano, string cnpj, DateTime dataInicio, DateTime dataDevolucao)
        {
            return "Veículo Alugado!";
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
            string conexao = @"Host=localhost;Port=5432;Username=postgres;Password=15975323;Database=AluguelVeiculos";
            string queryGetDiaria = @"SELECT valordiaria FROM planoslocacao WHERE dias=@plano";

            using (IDbConnection conn = new NpgsqlConnection(conexao))
            {
                decimal diaria = await conn.QueryFirstOrDefaultAsync<decimal>(queryGetDiaria, new { plano = plano });
                return diaria;                      
            }            
        }

        public async Task<bool> GetVeiculosDisponiveis()
        {
            string conexao = @"Host=localhost;Port=5432;Username=postgres;Password=15975323;Database=AluguelVeiculos";
            string queryGetDisponivel = @"SELECT alugado FROM CadastroVeiculo WHERE alugado=false";

            using (IDbConnection conn = new NpgsqlConnection(conexao))
            {
                var disponibilidade = await conn.QueryFirstOrDefaultAsync<bool>(queryGetDisponivel);

                if (disponibilidade != false)
                {
                    return false;
                }
                return true;
            }
        }
        
    }
}
    
