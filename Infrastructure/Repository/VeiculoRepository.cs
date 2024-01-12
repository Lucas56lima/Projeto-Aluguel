using Dapper;
using Domain.Commands;
using Domain.Entidades;
using Domain.Interface;
using System.Data.SqlClient;
using System.Numerics;


namespace Infrastructure.Repository
{
    public class VeiculoRepository: IVeiculoRepository
    {
        string conexao = @"Server=(localdb)\mssqllocaldb;Database=AluguelVeiculo;Trusted_Connection=True;MultipleActiveResultSets=True";
        public async Task<string> PostAsync(VeiculoCommand command)
        {
            string queryInsert = @"
            INSERT INTO Veiculo (VeiculoName,Placa,AnoFabricacao,TipoVeiculoId,Estado,FabricanteId) 
            VALUES (@VeiculoName,@Placa,@AnoFabricacao,@TipoVeiculoId,@Estado,@FabricanteId)";

            using (SqlConnection conn = new SqlConnection(conexao))
            {
                conn.Execute(queryInsert, new
                {
                    Placa = command.Placa,
                    AnoFabricacao = command.AnoFabricacao,
                    TipoVeiculoId = (int)command.TipoVeiculo,
                    VeiculoName = command.VeiculoName,
                    Estado = command.Estado,
                    FabricanteId = (int)command.Fabricante                   

                }); ;
            }

            return "Veículo cadastrado com sucesso!";

        }
        public void PostAsync()
        {

        }

        public void GetAsync()
        {

        }         

        

        public async Task<IEnumerable<VeiculoCommand>> GetDisponivel()
        {
            string queryInsert = @"SELECT * FROM Veiculo WHERE Alugado=0";

            using (SqlConnection conn = new SqlConnection(conexao))
            {
                return await conn.QueryAsync<VeiculoCommand>(queryInsert);
            }
            
        }

        public async Task<IEnumerable<VeiculoCommand>> GetAlugado()
        {
            string queryInsert = @"SELECT * FROM Veiculo WHERE Alugado=1";

            using (SqlConnection conn = new SqlConnection(conexao))
            {
                return await conn.QueryAsync<VeiculoCommand>(queryInsert);
            }

        }

    }
}
