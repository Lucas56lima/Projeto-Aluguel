namespace Domain.Commands
{
    public class EntregadorCommand
    {

        public int entregadorID { get; set; }
        public string nomeEntregador { get; set; }        
        public long cnpjEntregador { get; set; }
        public DateTime nascimentoEntregador { get; set; }
        public long numeroCNH { get; set; }
        public string tipoCNH{ get; set; }
        public string nomeArquivoCNH { get; set; }
        public Stream fotoCNH { get; set; }
    }
}
