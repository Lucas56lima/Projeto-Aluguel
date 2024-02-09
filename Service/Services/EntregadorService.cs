using Domain.Commands;
using Domain.Interface;
using Domain.ViewModel;

namespace Service.Services
{
    public class EntregadorService : IEntregadorService
    {
        private readonly IEntregadorRepository _repository;        
                
        public EntregadorService(IEntregadorRepository repository)
        {
            _repository = repository;            
        }        

        public async Task<AlugarVeiculoViewModel> PostAlugarAsync(string cnpj, AlugarVeiculoViewModel aluguelViewModel)
        {
            
            if(aluguelViewModel.plano == 0)
            {
                Console.WriteLine("Todos os campos são obrigatórios");
                return null;
            }            
            aluguelViewModel.dataInicio = aluguelViewModel.dataInicio.AddDays(1);
            var validaData = await VerificaDataMaior(aluguelViewModel.dataInicio, aluguelViewModel.dataDevolucao);
            if (validaData)
            {                
                Console.WriteLine("Data inicial não pode ser maior do que data de devolução!");
                return null;
            }

            var validacnpj = await ValidaCNPJ(cnpj);

            if (!validacnpj)
            {
                Console.WriteLine("CNPJ inexistente!");
                return null;               

            }

            var disponibilidade = await GetVeiculosDisponiveis();
            if (disponibilidade == 0)
            {
                Console.WriteLine("Não há veículos disponíveis!");
                return null;               
            }

            if(aluguelViewModel.plano > 7  & aluguelViewModel.plano < 15 ||aluguelViewModel.plano > 15 && aluguelViewModel.plano < 30 || aluguelViewModel.plano > 30) 
            {
                Console.WriteLine("Por gentileza insira plano com 7, 15 e 30 diárias!");
                return null;
            }

            var validaID = await GetEntregadorID(cnpj);

            if(validaID == 0)
            {
                Console.WriteLine("CNPJ incorreto!");
                return null;
            }            

            return await _repository.PostAlugarAsync(cnpj,aluguelViewModel);
        }

        public async Task<string> PostEntregadorAsync(EntregadorCommand command)   
        {
            if(command == null)
            {
                return "Todos os dados são obrigatórios";
            }

            var validaCNPJ = await ValidaCNPJ(command.cnpjEntregador);
            var validaCNH = await ValidaCNH(command.numeroCNH);
            if (validaCNPJ)
            {
                return "CNPJ já cadastrado";
            }

            if (validaCNH != null)
            {
                return "CNH já cadastrada";
            }
            
            if(command.tipoCNH != "A" && command.tipoCNH != "AB")
            {
                return "Somente habilitações A e AB são permitidas";
            }

            return await _repository.PostEntregadorAsync(command);        
        }

        public async Task<string> PutImageAsync(string cnpj, ImagemCommand imagemCommand)
        {
            if (imagemCommand == null)
            {
                return "Por favor, insira uma imagem";
            }

            var validaCNPJ = await ValidaCNPJ(cnpj);

            if (!validaCNPJ)
            {
                return "CNPJ inválido, por gentileza tente novamente";
            }                      

            return await _repository.PutImageAsync(cnpj,imagemCommand);
        }

        public async Task<Stream> RecuperarFotoCNHAsync(string nomeArquivo)
        {
            return await _repository.RecuperarFotoCNHAsync(nomeArquivo);
        }

        public async Task<string> SalvarFotoCNHAsync(string nomeArquivo,Stream fotoCNH)
        {
            return await _repository.SalvarFotoCNHAsync(nomeArquivo,fotoCNH);
        }

        public async Task<string> ValidaCNH(string cnh)
        {
            return await _repository.ValidaCNH(cnh);
        }

        public async Task<bool> ValidaCNPJ(string cnpj)
        {
            return await _repository.ValidaCNPJ(cnpj);
        }

        public async Task<bool> VerificaDataMaior(DateTime dataInicio, DateTime dataDevolucao)
        {
            return await _repository.VerificaDataMaior(dataInicio, dataDevolucao);
        }

        public async Task<decimal> GetValorDiaria(int plano)
        {
            return await _repository.GetValorDiaria(plano);
        }

        public async Task<int> GetVeiculosDisponiveis()
        {
            return await _repository.GetVeiculosDisponiveis();
        }

        public async Task<AlugarVeiculoViewModel> SimularAluguel(int plano, string cnpj, DateTime dataInicio, DateTime dataDevolucao)
        {
            return await _repository.SimularAluguel(plano, cnpj, dataInicio, dataDevolucao);
        }

        private void MostrarSimulacao(AlugarVeiculoViewModel simulacao)
        {
            // Aqui você pode implementar a lógica para mostrar a simulação.
            // Pode ser uma janela de diálogo, uma mensagem no console, ou qualquer outra forma de apresentação que desejar.
            Console.WriteLine("Simulação de Aluguel:");
            Console.WriteLine($"Plano: {simulacao.plano}");
            Console.WriteLine($"Valor Diária: {simulacao.valorDiaria}");
            Console.WriteLine($"Data Início: {simulacao.dataInicio}");
            Console.WriteLine($"Data Devolução: {simulacao.dataDevolucao}");
            Console.WriteLine($"Data Previsão Devolução: {simulacao.dataPrevisaoDevolucao}");
            Console.WriteLine($"Valor Total: {simulacao.valorTotal}");
        }

        public async Task<int> GetEntregadorID(string cnpj)
        {
            return await _repository.GetEntregadorID(cnpj);
        }
    }


}
