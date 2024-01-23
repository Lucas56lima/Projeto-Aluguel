using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades
{
    public class ClienteCommand
    {

        public int PessoaId { get; set; }
        public string Nome { get; set; }        

        public int Cpf { get; set; }

        public int Contato { get; set; }

        public string Nascimento { get; set; }

        public int Habilitacao { get; set; }
        public string Estado { get; set; }

    }
}
