using Domain.Commands;
using Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost]
        [Route("CadastrarVeiculo")]
        public async Task<IActionResult> PostAsyncVeiculo([FromBody] VeiculoCommand command)
        {
            return Ok(await _adminService.PostAsyncVeiculo(command));
        }

        [HttpGet]
        [Route("ConsultarVeiculos")]
        public async Task<IActionResult> GetAsyncVeiculo(string?placa)
        {
            return Ok(await _adminService.GetAsyncVeiculo(placa));
        }

        [HttpPut]
        [Route("AlterarCadastroVeiculo")]

        public async Task<IActionResult> PutAsyncVeiculo(int veiculoID,string placa)
        {
            return Ok(await _adminService.PutAsyncVeiculo(veiculoID, placa));
        }

        [HttpDelete]
        [Route("InativarCadastroVeiculo")]

        public async Task<IActionResult> DeleteAsyncVeiculo(int veiculoID)
        {
            return Ok(await _adminService.DeleteAsyncVeiculo(veiculoID));
        }
    }
}        

