using Domain.Commands;
using Domain.Enums;
using Domain.ViewModel;

namespace Domain.Interface
{
    public interface IVeiculoRepository
    {
        Task<string> PostAsync(VeiculoCommand command);        

        Task<VeiculoPrecoCommand> GetAsync(ETipoVeiculo tipoVeiculo);

        Task<IEnumerable<VeiculoCommand>> GetDisponivel();

        Task<IEnumerable<VeiculoCommand>> GetAlugado();

        Task<bool> ValidarDisponivel(string placaVeiculo);

        Task<bool> VerificaDataMaior(DateTime dataRetirada, DateTime dataDevolucao);

        Task<SimularVeiculoAluguelViewModel>SimularAluguel(int diasSimulados, ETipoVeiculo tipoVeiculo);

        Task<AlugarVeiculoViewModelInput> AlugarVeiculo(AlugarVeiculoViewModelInput input);

        //Task<object?> GetAsync(VeiculoCommand preco);

    }
}
