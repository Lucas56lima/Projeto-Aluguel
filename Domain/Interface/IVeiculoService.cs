using Domain.Commands;

namespace Domain.Interface
{
    public interface IVeiculoService
    {        
        Task<string> PostAsyncVeiculo(VeiculoCommand command);
        Task<IEnumerable<VeiculoCommand>> GetAsyncVeiculo(string?placa);
        Task<string> PutAsyncVeiculo(int veiculoID, string placa);
        Task<string> DeleteAsyncVeiculo(int veiculoID);
        Task<bool> VerificaPlaca(string placa);        
    }
}
