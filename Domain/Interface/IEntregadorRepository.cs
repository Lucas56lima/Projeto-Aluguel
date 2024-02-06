using Domain.Commands;

namespace Domain.Interface
{
    public interface IEntregadorRepository
    {
        Task<string> PostAsync(EntregadorCommand command);
        
    }
}
