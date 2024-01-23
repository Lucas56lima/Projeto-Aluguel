using Domain.Commands;
using Domain.Enums;
using Domain.Interface;
using Domain.ViewModel;
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
        public async Task<IActionResult> GetAsync(int DiasSimulacao,ETipoVeiculo tipoVeiculo)
        {
            return Ok(await _veiculoService.SimularAluguel( DiasSimulacao, tipoVeiculo));
        }

        [HttpPost]
        [Route("Alugar")]

        public async Task<IActionResult> PostAsync([FromBody] AlugarVeiculoViewModelInput input)
        {
            
            return Ok(await _veiculoService.AlugarVeiculo(input));
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
