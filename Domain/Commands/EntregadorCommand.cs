namespace Domain.Commands
{
    public class EntregadorCommand
    {
        public int entregadorID { get; set; }
        public string nomeEntregador { get; set; }        
        public string cnpjEntregador { get; set; }
        public DateTime nascimentoEntregador { get; set; }
        public string numeroCNH { get; set; }
        public string tipoCNH{ get; set; }
        public bool locacao { get; set; } = false;
    }

    public class ImagemCommand
    {
        public string nomeArquivo { get; set; }       
    }   
}
