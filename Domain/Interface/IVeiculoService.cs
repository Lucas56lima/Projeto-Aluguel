using Domain.Commands;

namespace Domain.Interface
{
    public interface IVeiculoService
    {
        Task PostAsync(VeiculoCommand command);

        void PostAsync();

        void GetAsync();

    }
}
