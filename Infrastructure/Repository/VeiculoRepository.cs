using Dapper;
using Domain.Commands;
using Domain.Entidades;
using Domain.Enums;
using Domain.Interface;
using Domain.ViewModel;
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

        public async Task<VeiculoPrecoCommand>GetAsync(ETipoVeiculo tipoVeiculo)
        {
           
            string querySimula = @"SELECT Preco FROM VeiculoPreco WHERE TipoVeiculoId=@TipoVeiculoId";
            
            using (SqlConnection conn = new SqlConnection(conexao))
            {

                return conn.QueryAsync<VeiculoPrecoCommand>(querySimula, new
                {
                    TipoVeiculo = tipoVeiculo
                }).Result.FirstOrDefault();

            }

            
        }                       

        public async Task<IEnumerable<VeiculoCommand>> GetDisponivel()
        {
            string queryDisponivel = @"SELECT * FROM Veiculo WHERE Alugado=0";

            using (SqlConnection conn = new SqlConnection(conexao))
            {
                return await conn.QueryAsync<VeiculoCommand>(queryDisponivel);
            }
            
        }

        public async Task<IEnumerable<VeiculoCommand>> GetAlugado()
        {
            string queryAlugado = @"SELECT * FROM Veiculo WHERE Alugado=1";

            using (SqlConnection conn = new SqlConnection(conexao))
            {
                return await conn.QueryAsync<VeiculoCommand>(queryAlugado);
            }

        }

        public Task<SimularVeiculoAluguelViewModel> SimularAluguel(int diasSimulados, ETipoVeiculo tipoVeiculo)
        {
            throw new NotImplementedException();
        }

        public Task<AlugarVeiculoViewModelInput> AlugarVeiculo(AlugarVeiculoViewModelInput input)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ValidarDisponivel(string placaVeiculo)
        {
            string queryDisponivel = @"SELECT Alugado FROM Veiculo WHERE Placa=@placaVeiculo AND Alugado=0";

            using (SqlConnection conn = new SqlConnection(conexao))
            {
                return conn.QueryAsync<bool>(queryDisponivel, new
                {
                    placaVeiculo = placaVeiculo
                }).Result.FirstOrDefault();                   
                
            }

        }

        public async Task<bool> VerificaDataMaior(DateTime dataRetirada, DateTime dataDevolucao)
        {

            if (dataRetirada > dataDevolucao)
            {
                return false;
            }
            return true;

        }

    }
}
