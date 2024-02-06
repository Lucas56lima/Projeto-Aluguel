using Domain.Commands;
using Domain.Interface;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> PostAsyncVeiculo([FromBody] VeiculoCommand command)
        {
            return Ok(await _veiculoService.PostAsyncVeiculo(command));
        }

        [HttpGet]
        [Route("ConsultarVeiculos")]
        public async Task<IActionResult> GetAsyncVeiculo(string?placa)
        {
            return Ok(await _veiculoService.GetAsyncVeiculo(placa));
        }

        [HttpPut]
        [Route("AlterarCadastroVeiculo")]

        public async Task<IActionResult> PutAsyncVeiculo(int veiculoID,string placa)
        {
            return Ok(await _veiculoService.PutAsyncVeiculo(veiculoID, placa));
        }

        [HttpDelete]
        [Route("InativarCadastroVeiculo")]

        public async Task<IActionResult> DeleteAsyncVeiculo(int veiculoID)
        {
            return Ok(await _veiculoService.DeleteAsyncVeiculo(veiculoID));
        }
    }
}        

