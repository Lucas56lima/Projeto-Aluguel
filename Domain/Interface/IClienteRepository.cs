using Domain.Commands;

namespace Domain.Interface
{
    public interface IClienteRepository
    {
        Task<string> PostAsync(ClienteCommand command);
    }
}
