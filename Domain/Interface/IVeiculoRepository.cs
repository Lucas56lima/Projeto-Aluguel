using Domain.Commands;
using Domain.Entidades;

namespace Domain.Interface
{
    public interface IVeiculoRepository
    {
        Task<string> PostAsync(VeiculoCommand command);

        void PostAsync();

        void GetAsync();

        Task<IEnumerable<VeiculoCommand>> GetDisponivel();

        Task<IEnumerable<VeiculoCommand>> GetAlugado();


    }
}
