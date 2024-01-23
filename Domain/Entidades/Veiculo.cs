using Domain.Enums;

namespace Domain.Entidades
{
    public class Veiculo
    {
        public int  VeiculoId {get;set;}
        public string VeiculoName { get;set;}
        public string Placa { get;set;}
        public int AnoFabricacao { get;set;}
        public ETipoVeiculo TipoVeiculo { get;set;}
        public string Estado {  get;set;}
        public EFabricante Fabricante { get;set;}

        public decimal Preco { get; set; }
        public bool Alugado { get;set;}

    }
}
