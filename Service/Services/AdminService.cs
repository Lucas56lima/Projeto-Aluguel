using Domain.Commands;
using Domain.Interface;

namespace Service.Services
{
    public class AdminService : IAdminService
    {
        // Injeção de dependência
        private readonly IAdminRepository _repository;

        public AdminService(IAdminRepository repository)
        {

            _repository = repository;

        }

        public async Task<IEnumerable<VeiculoCommand>> GetAsyncVeiculo(string? placa)
        {
;            return await _repository.GetAsyncVeiculo(placa);
        }

        public async Task<string> PostAsyncVeiculo(VeiculoCommand command)
        {

            if (command == null)
            {
                return "Todos os campos são obrigatórios";
            }
            var verificaPlaca = await _repository.VerificaPlaca(command.placaVeiculo);
            if (verificaPlaca)
            {
                return "Placa já cadastrada";
            }

            if(command.placaVeiculo.Length < 7)
            {
                return "Placa deve conter 7 caracteres";
            }

            return await _repository.PostAsyncVeiculo(command);
        }

        public async Task<string> PutAsyncVeiculo(int veiculoID, string placa)
        {
            return await _repository.PutAsyncVeiculo(veiculoID, placa);
        }

        public async Task<string> DeleteAsyncVeiculo(int veiculoID)
        {
            return await _repository.DeleteAsyncVeiculo(veiculoID);
        }

        public async Task<bool> VerificaPlaca(string placa)
        {
            return await _repository.VerificaPlaca(placa);
        }

        public async Task<string> PostAsyncPedido(PedidoCommand pedidoCommand)
        {
            return await _repository.PostAsyncPedido(pedidoCommand);
        }

        public async Task<IEnumerable<EntregadorCommand>> GetEntregadoresDisponiveis()
        {
            return await _repository.GetEntregadoresDisponiveis();
        }
    }
}
