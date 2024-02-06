using System.Data;
using Dapper;
using Domain.Commands;
using Domain.Interface;
using Npgsql;


namespace Repository.Repository
{
    public class VeiculoRepository: IVeiculoRepository
    {
        string conexao = @"Host=localhost;Port=5432;Username=postgres;Password=15975323;Database=Veiculo";              

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

        public async Task<IEnumerable<VeiculoCommand>> GetAsyncVeiculo(string?placa)
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
            string conexao = @"Host=localhost;Port=5432;Username=postgres;Password=15975323;Database=Veiculo";

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
                conn.Execute(queryUpdate, new {veiculoID = veiculoID,placa = placa});
            }

            return "Placa atualizada com sucesso!";
        }

        public async Task<string> DeleteAsyncVeiculo(int veiculoID)
        {
            string queryInativate = @"
            DELETE FROM CadastroVeiculo WHERE veiculoID=@veiculoID AND alugado=False";

            using (IDbConnection conn = new NpgsqlConnection(conexao))
            {
                conn.Execute(queryInativate, new { veiculoID = veiculoID});
            }

            return "Cadastro excluído com sucesso!";
        }
    }
}
