using CreditCardValidator;
using Domain.Commands;
using Domain.Enums;
using Domain.Interface;
using Domain.ViewModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Service.Services
{
    public class VeiculoService : IVeiculoService
    {
        // Injeção de dependência
        private readonly IVeiculoRepository _repository;

        public VeiculoService(IVeiculoRepository repository)
        {

            _repository = repository;

        }
      

        public async Task<string> PostAsync(VeiculoCommand command)
        {

            // To do
            // Incluir validação, só podem cadastrar veículos com até 5 anos de uso

            //To do
            //Incluir somente carros do tipo SUV,Sedan e Hatch
            int anoAtual = DateTime.Now.Year;            

            if (command == null )
            {
                return "Todos os campos são obrigatórios";
            }

            if (command.TipoVeiculo == ETipoVeiculo.Pickup)
            {

                return "Tipo de carro não permitido";
            }            

            if (anoAtual - command.AnoFabricacao  > 5) {

                return "Não é permitido carro com mais de 5 anos";
            }

            if(anoAtual - command.AnoFabricacao < 0)
            {
                return "Ano inserido maior do que ano atual!";
            }

            return await _repository.PostAsync(command);
        }

        public void PostAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<VeiculoCommand>> GetDisponivel()
        {
            return await _repository.GetDisponivel();
        }

        public async Task<IEnumerable<VeiculoCommand>> GetAlugado()
        {
            return await _repository.GetAlugado();
        }

        public Task<VeiculoPrecoCommand> GetAsync(ETipoVeiculo tipoVeiculo)
        {
            throw new NotImplementedException();
        }



        public async Task<SimularVeiculoAluguelViewModel> SimularAluguel(int diasSimulados, ETipoVeiculo tipoVeiculo)
        {
            var veiculoPreco = await _repository.GetAsync(tipoVeiculo);
            double taxaEstadual = 10.50;
            double taxaFederal = 3.5;
            double taxamunicipal = 13.5;

            var simulacao = new SimularVeiculoAluguelViewModel();
            if (tipoVeiculo == ETipoVeiculo.SUV && diasSimulados > 45)
            {
                return null;
            }
            simulacao.DiasSimulados = diasSimulados;
            simulacao.Taxas = (decimal)(taxamunicipal + taxaEstadual + taxaFederal);
            simulacao.TipoVeiculo = tipoVeiculo;
            simulacao.ValorDiaria = veiculoPreco.Preco;
            simulacao.ValorTotal = (diasSimulados * veiculoPreco.Preco) + simulacao.Taxas;
            return simulacao;
        }

        public async Task<AlugarVeiculoViewModelInput> AlugarVeiculo(AlugarVeiculoViewModelInput input)
        {
            CreditCardDetector detector = new CreditCardDetector(Convert.ToString(input.Cartao.Numero));
            var numero = detector.CardNumber; // => 4012888888881881

            detector.IsValid(); // => True
            detector.IsValid(CardIssuer.Maestro); // => False

            var bandeira = detector.Brand; // => CardIssuer.Visa
            var bandeiraName =  detector.BrandName; // => Visa

            var categoria = detector.IssuerCategory; // => Banking and financial

            var veiculoDisponivel = await ValidarDisponivel(input.PlacaVeiculo);
            var alugar = new AlugarVeiculoViewModelInput();
            if (veiculoDisponivel == false)
            {

                return null;
            }

            var verificarDatas = await VerificaDataMaior(input.DataRetirada, input.DataDevolucao);

            if (verificarDatas)
            {

                return null;
            }

            return alugar;           

        }

        public Task<bool> ValidarDisponivel(string placaVeiculo)
        {
           return _repository.ValidarDisponivel(placaVeiculo);
            
        }

        public Task<bool> VerificaDataMaior(DateTime dataRetirada, DateTime dataDevolucao)
        {
            return _repository.VerificaDataMaior(dataRetirada, dataDevolucao);                             

        }       

        
    }
}
