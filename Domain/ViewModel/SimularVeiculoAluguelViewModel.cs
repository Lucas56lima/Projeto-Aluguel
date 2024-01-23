using Domain.Enums;

namespace Domain.ViewModel
{
    public class SimularVeiculoAluguelViewModel
    {

        public decimal ValorTotal { get; set; }
        public decimal ValorDiaria {  get; set; }

        public ETipoVeiculo TipoVeiculo { get; set; }
        public decimal Taxas { get; set; }
        public int DiasSimulados { get; set; }

    }
}
