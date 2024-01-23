using Domain.Commands;

namespace Domain.Interface
{
    public interface IClienteService
    {
        Task<string> PostAsync(ClienteCommand command);
    }
}
