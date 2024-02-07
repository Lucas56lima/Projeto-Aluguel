namespace Domain.Entidades
{
    public class ClienteCommand
    {
        public int entregadorID{ get; set; }
        public string nomeEntregador { get; set; }
        public string cnpjEntregador { get; set; }
        public DateTime nascimentoEntregador { get; set; }
        public string numeroCNH { get; set; }
        public string tipoCNH { get; set; }        
    }

    public class ImagemCommand
    {
        public string nomeArquivo { get; set; }        
    }    
}
