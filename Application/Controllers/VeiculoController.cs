using Domain.Commands;
using Domain.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Services;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculoController : ControllerBase
    {

        private readonly IVeiculoService _veiculoService;

        public VeiculoController(IVeiculoService veiculoService)
        {
            _veiculoService = veiculoService;
        }

        [HttpPost]
        [Route("CadastrarVeiculo")]
        public async Task<IActionResult> PostAsync([FromBody] VeiculoCommand command)
        {            
            return Ok(await _veiculoService.PostAsync(command));
        }


        [HttpGet]
        [Route("SimularAluguel")]
        public IActionResult GetAsync()
        {
            return Ok();
        }

        [HttpPost]
        [Route("Alugar")]

        public IActionResult PostAsync()
        {
            return Ok();
        }

        [HttpGet]
        [Route("Disponivel")]

        public async Task<IActionResult> GetDisponivel()
        {
            return Ok(await _veiculoService.GetDisponivel());
        }

        [HttpGet]
        [Route("Alugado")]

        public async Task<IActionResult> GetAlugado()
        {
            return Ok(await _veiculoService.GetAlugado());
        }

    }
}
