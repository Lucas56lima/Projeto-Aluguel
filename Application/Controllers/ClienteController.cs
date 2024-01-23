using Domain.Commands;
using Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteservice;

        public ClienteController(IClienteService clienteService)
        {
            _clienteservice = clienteService;
            
        }

        [HttpPost]
        [Route("CadastrarCliente")]
        public async Task<IActionResult> PostAsync([FromBody] ClienteCommand command)
        {
            return Ok(await _clienteservice.PostAsync(command));
        }

    }
}

