using Domain.Interface;

namespace Repository.Repository
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly string _caminhoRaiz;

        public LocalFileStorageService(string caminhoRaiz)
        {
            _caminhoRaiz = caminhoRaiz;
        }

        public async Task<string> SalvarFotoCNHAsync(string nomeArquivo, Stream fotoCNH)
        {
            try
            {
                // Combinar o caminho raiz com o nome do arquivo para obter o caminho completo
                string caminhoCompleto = Path.Combine(_caminhoRaiz, nomeArquivo);

                // Verificar se o diretório de destino existe, se não, criá-lo
                string diretorioDestino = Path.GetDirectoryName(caminhoCompleto);
                if (!Directory.Exists(diretorioDestino))
                {
                    Directory.CreateDirectory(diretorioDestino);
                }

                // Salvar a foto da CNH no disco
                using (var fileStream = new FileStream(caminhoCompleto, FileMode.Create))
                {
                    await fotoCNH.CopyToAsync(fileStream);
                }

                return caminhoCompleto;
            }
            catch (Exception ex)
            {
                // Lidar com erros ou lançar para camadas superiores
                throw new Exception($"Erro ao salvar foto da CNH: {ex.Message}");
            }
        }

        public async Task<Stream> RecuperarFotoCNHAsync(string nomeArquivo)
        {
            try
            {
                // Combinar o caminho raiz com o nome do arquivo para obter o caminho completo
                string caminhoCompleto = Path.Combine(_caminhoRaiz, nomeArquivo);

                // Verificar se o arquivo existe
                if (!File.Exists(caminhoCompleto))
                {
                    throw new FileNotFoundException($"Arquivo {nomeArquivo} não encontrado.");
                }

                // Abrir o arquivo e retornar a stream
                return new FileStream(caminhoCompleto, FileMode.Open);
            }
            catch (Exception ex)
            {
                // Lidar com erros ou lançar para camadas superiores
                throw new Exception($"Erro ao recuperar foto da CNH: {ex.Message}");
            }
        }
    }
}


