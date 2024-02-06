using Domain.Commands;

namespace Domain.Interface
{
    public interface IEntregadorService
    {
        Task<string> PostAsync(EntregadorCommand command);        
    }
}
