using Domain.Commands;
using Domain.Enums;
using Domain.ViewModel;
using System.Numerics;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IVeiculoService
    {
        Task<string> PostAsync(VeiculoCommand command);
        
        Task<VeiculoPrecoCommand> GetAsync(ETipoVeiculo tipoVeiculo);

        Task<IEnumerable<VeiculoCommand>> GetDisponivel();

        Task<IEnumerable<VeiculoCommand>> GetAlugado();

        Task<bool> ValidarDisponivel(string placaVeiculo);
        Task<SimularVeiculoAluguelViewModel> SimularAluguel(int diasSimulados, ETipoVeiculo tipoVeiculo);

        Task <AlugarVeiculoViewModelInput> AlugarVeiculo(AlugarVeiculoViewModelInput input);

        Task<bool> VerificaDataMaior(DateTime dataRetirada, DateTime dataDevolucao);

    }
}
