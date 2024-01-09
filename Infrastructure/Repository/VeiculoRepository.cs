using Dapper;
using Domain.Entidades;
using Domain.Interface;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Repository
{
    public class VeiculoRepository: IVeiculoRepository
    {
        private string stringConnection = @"";
        public async Task<string> PostAsync(Veiculo command)
        {
            string queryInsert = @"
INSERT INTO Veiculo(INSERT INTO Veiculo (Placa,AnoFabricacao,TipoVeiculoId,VeiculoName,Estado,FabricanteId) 
VALUES(@Placa,@AnoFabricacao,@TipoVeiculoId,@VeiculoName,@Estado,@FabricanteId))";

            using (var conn = new SqlConnection())
            {
                conn.Execute(queryInsert, new
                {
                    Placa = command.Placa,
                    AnoFabricacao = command.AnoFabricacao,
                    TipoVeiculoId = command.TipoVeiculo,
                    VeiculoName = command.VeiculoName,
                    Estado = command.Estado,
                    FabricanteId = command.Fabricante

                }) ;
            }

            return "Veículo cadastrado com sucesso!";

        }
        public void PostAsync()
        {

        }

        public void GetAsync()
        {

        }
    }
}
