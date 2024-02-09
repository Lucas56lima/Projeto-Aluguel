namespace Domain.ViewModel
{
    public class AlugarVeiculoViewModel
    {
        public DateTime dataInicio { get; set; }
        public int plano { get; set; }
        public DateTime dataPrevisaoDevolucao { get; set; }
        public DateTime dataDevolucao { get; set; }
        public decimal valorDiaria { get; set; }
        public decimal valorTotal { get; set; }

    }
}
