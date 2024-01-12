using Domain.Commands;

namespace Domain.Interface
{
    public interface IVeiculoService
    {
        Task<string> PostAsync(VeiculoCommand command);

        void PostAsync();

        void GetAsync();

        Task<IEnumerable<VeiculoCommand>> GetDisponivel();

        Task<IEnumerable<VeiculoCommand>> GetAlugado();

    }
}
