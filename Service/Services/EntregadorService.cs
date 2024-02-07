using System.Globalization;
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

        public async Task<string> PostAlugarAsync(int plano, string cnpj, DateTime dataInicio, DateTime dataDevolucao, AlugarVeiculoViewModel alugarViewModel)
        {
            
            var validaData = await VerificaDataMaior(alugarViewModel.dataInicio, alugarViewModel.dataDevolucao);
            if (validaData)
            {
                return "Data inicial não pode ser maior que Data de Devolução";
            }

            var validacnpj = await ValidaCNPJ(cnpj);

            if (!validacnpj)
            {
                return "CNPJ inexistente";
            }

            var disponibilidade = await GetVeiculosDisponiveis();
            if (!disponibilidade)
            {
                return "Não há Veículos disponíveis";
            }

            if(alugarViewModel.plano > 7 || alugarViewModel.plano > 15 || alugarViewModel.plano > 30) 
            {
                return "Por gentileza indique plano de 7, 15 ou 30 diárias";
            }

            var alugar = await SimularAluguel(plano, cnpj, dataInicio,dataDevolucao);

            return await _repository.PostAlugarAsync(plano,cnpj,dataInicio,dataDevolucao,alugar);
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

            if (validaCNH)
            {
                return "CNH já cadastrada";
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

        public async Task<bool> ValidaCNH(string cnh)
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

        public async Task<bool> GetVeiculosDisponiveis()
        {
            return await _repository.GetVeiculosDisponiveis();
        }

        public async Task<AlugarVeiculoViewModel> SimularAluguel(int plano, string cnpj, DateTime dataInicio, DateTime dataDevolucao)
        {
            decimal taxaPlano7 = 0.2m;
            decimal taxaPlano15 = 0.4m;
            decimal taxaPlano30 = 0.6m;

            var simulacao = new AlugarVeiculoViewModel();
            var diaria = await GetValorDiaria(plano);

            simulacao.plano = plano;
            simulacao.dataInicio = dataInicio;
            simulacao.dataDevolucao = dataDevolucao;

            int diferencaDias = (dataDevolucao - dataInicio).Days;

            if (diferencaDias > 0 && plano == 7)
            {
                simulacao.valorDiaria = diaria + (diaria * taxaPlano7);
                simulacao.dataPrevisaoDevolucao = dataInicio.AddDays(7);
            }
            else if (diferencaDias > 0 && plano == 15)
            {
                simulacao.valorDiaria = diaria + (diaria * taxaPlano15);
                simulacao.dataPrevisaoDevolucao = dataInicio.AddDays(15);
            }
            else if (diferencaDias > 0 && plano == 30)
            {
                simulacao.valorDiaria = diaria + (diaria * taxaPlano30);
                simulacao.dataPrevisaoDevolucao = dataInicio.AddDays(30);
            }

            simulacao.valorTotal = simulacao.valorDiaria * plano;

            return simulacao;
        }
    }
}
