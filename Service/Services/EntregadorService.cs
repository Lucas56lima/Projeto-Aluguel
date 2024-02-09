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

        public async Task<string> PostAlugarAsync(int plano, string cnpj, DateTime dataInicio, DateTime dataDevolucao)
        {
            
            var validaData = await VerificaDataMaior(dataInicio, dataDevolucao);
            if (validaData)
            {
                return "Data inicial não pode ser maior do que data de devolução!";
            }

            var validacnpj = await ValidaCNPJ(cnpj);

            if (validacnpj < 0)
            {
                return "CNPJ inexistente!";
            }

            var disponibilidade = await GetVeiculosDisponiveis();
            if (!disponibilidade)
            {
               return "Não há veículos disponíveis!";
            }

            if(plano > 7 || plano > 15 || plano > 30) 
            {
                return "Por gentileza insira plano com 7, 15 e 30 diárias!";
            }

            var alugar = await SimularAluguel(plano, cnpj, dataInicio,dataDevolucao);

            MostrarSimulacao(alugar);

            return await _repository.PostAlugarAsync(plano,cnpj,dataInicio,dataDevolucao);
        }

        public async Task<string> PostEntregadorAsync(EntregadorCommand command)   
        {
            if(command == null)
            {
                return "Todos os dados são obrigatórios";
            }

            var validaCNPJ = await ValidaCNPJ(command.cnpjEntregador);
            var validaCNH = await ValidaCNH(command.numeroCNH);
            if (validaCNPJ >= 0)
            {
                return "CNPJ já cadastrado";
            }

            if (validaCNH != null)
            {
                return "CNH já cadastrada";
            }
            
            if(validaCNH != "A" || validaCNH != "AB" && validaCNH != null)
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

            if (validaCNPJ < 0)
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

        public async Task<int> ValidaCNPJ(string cnpj)
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

            diferencaDias = (dataDevolucao - simulacao.dataDevolucao).Days;

            simulacao.valorTotal = simulacao.valorDiaria * plano;

            if (diferencaDias > 0)
            {
                decimal acrescimo = 50 * diferencaDias;
                simulacao.valorTotal += acrescimo;
            }            

            return simulacao;
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
    }


}
