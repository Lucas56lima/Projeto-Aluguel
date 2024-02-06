using Domain.Commands;
using Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntregadorController : ControllerBase
    {
        private readonly IEntregadorService _entregadorservice;

        public EntregadorController(IEntregadorService entregadorService)
        {
            _entregadorservice = entregadorService;
            
        }

        [HttpPost]
        [Route("CadastrarEntregador")]
        public async Task<IActionResult> PostAsync([FromBody] EntregadorCommand command)
        {           
                     
          return Ok(await _entregadorservice.PostAsync(command));
          
        }

    }
}

