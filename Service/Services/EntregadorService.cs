using Domain.Commands;
using Domain.Interface;

namespace Service.Services
{
    public class EntregadorService : IEntregadorService
    {
        private readonly IEntregadorRepository _repository;
        private readonly IFileStorageService _fileStorageService;
                
        public EntregadorService(IEntregadorRepository repository, IFileStorageService fileStorageService)
        {
            _repository = repository;
            _fileStorageService = fileStorageService;
        }
        public async Task<string> PostAsync(EntregadorCommand command)   
        {
            string caminhoFotoCNH = await _fileStorageService.SalvarFotoCNHAsync(command.nomeArquivoCNH, command.fotoCNH);

            return caminhoFotoCNH;         
        }
        
    }
}
