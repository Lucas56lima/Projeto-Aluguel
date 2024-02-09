using Domain.Commands;
using Domain.Interface;
using Domain.ViewModel;
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
        public async Task<IActionResult> PostEntregadorAsync([FromBody] EntregadorCommand command)
        {       
                    
          return Ok(await _entregadorservice.PostEntregadorAsync(command));          
        }
        [HttpPut]
        [Route("InserirImagemCNH")]

        public async Task<IActionResult> PutImageAsync(string cnpj, [FromBody] ImagemCommand imagemCommand)
        {
            return Ok(await _entregadorservice.PutImageAsync(cnpj, imagemCommand));
        }

        [HttpGet]
        [Route("SimularAluguel")]
        public async Task<IActionResult> SimularAluguel(int plano, string cnpj, DateTime dataInicio, DateTime dataDevolucao)
        {
            return Ok(await _entregadorservice.SimularAluguel(plano, cnpj, dataInicio,dataDevolucao));
        }

        [HttpPost]
        [Route("AlugarVeiculo")]

        public async Task<IActionResult> PostAlugarAsync(string cnpj,AlugarVeiculoViewModel aluguelViewModel)
        {
            return Ok(await _entregadorservice.PostAlugarAsync(cnpj, aluguelViewModel));
        }
    }
}

