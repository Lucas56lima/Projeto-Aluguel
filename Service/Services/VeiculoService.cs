using Domain.Commands;
using Domain.Enums;
using Domain.Interface;

namespace Service.Services
{
    public class VeiculoService : IVeiculoService
    {
        public void GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task PostAsync(VeiculoCommand command)
        {
            // To do
            // Incluir validação, só podem cadastrar veículos com até 5 anos de uso

            //To do
            //Incluir somente carros do tipo SUV,Sedan e Hatch

            if (command == null )
            {
                throw new ArgumentNullException();
            }

            if (command.TipoVeiculo == ETipoVeiculo.Pickup)

            {
                Console.WriteLine("Não cadastrou!");
                throw new ArgumentNullException();
            }
            else
            {
                Console.WriteLine("Cadastrou!");
            }

            throw new NotImplementedException();

        }

        public void PostAsync()
        {
            throw new NotImplementedException();
        }
    }
}
