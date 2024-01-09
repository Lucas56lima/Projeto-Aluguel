using Domain.Entidades;

namespace Domain.Interface
{
    public interface IVeiculoRepository
    {
        Task<string> PostAsync(Veiculo command);

        void PostAsync();

        void GetAsync();

    }
}
