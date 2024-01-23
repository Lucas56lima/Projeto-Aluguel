using Domain.Commands;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repository;

        public ClienteService(IClienteRepository repository)
        {

            _repository = repository;

        }
        public Task<string> PostAsync(ClienteCommand command)
        {
            return _repository.PostAsync(command);            
        }
    }
}
