using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IFileStorageService
    {
        Task<string> SalvarFotoCNHAsync(string nomeArquivo, Stream fotoCNH);
        Task<Stream> RecuperarFotoCNHAsync(string nomeArquivo);
    }
}
