using Dapper;
using Domain.Commands;
using Domain.Interface;
using System.Data.SqlClient;

namespace Infrastructure.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        string conexao = @"Server=(localdb)\mssqllocaldb;Database=AluguelVeiculo;Trusted_Connection=True;MultipleActiveResultSets=True";
        public async Task<string> PostAsync(ClienteCommand command)
        {
            string queryInsert = @"
            INSERT INTO Clientes (Nome,Cpf,Contato,Nascimento,Habilitacao,Estado) 
            VALUES (@Nome,@Cpf,@Contato,@Nascimento,@Habilitacao,@Estado)";

            using (SqlConnection conn = new SqlConnection(conexao))
            {
                conn.Execute(queryInsert, new
                {
                    Nome = command.Nome,
                    Cpf = command.Cpf,
                    Contato = command.Contato,
                    Nascimento = command.Nascimento,
                    Habilitacao = command.Habilitacao,
                    Estado = command.Estado

                });
            }

            return "Cliente cadastrado com sucesso!";

        }
    }
}
    
