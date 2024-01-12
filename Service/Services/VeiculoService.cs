using Domain.Commands;
using Domain.Enums;
using Domain.Interface;

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


        public void GetAsync()
        {
            throw new NotImplementedException();
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
        
    }
}
