using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversidadeRH.Aplicacao.DTOs;
using UniversidadeRH.Aplicacao.Interfaces;

namespace UniversidadeRH.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [Tags("02. Módulo de Benefícios")]
    public class BeneficiosController : ControllerBase
    {
        private readonly IBeneficioService _service;

        public BeneficiosController(IBeneficioService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarBeneficioDto dto)
        {
            // Agora passamos o 'dto' direto, não mais propriedade por propriedade
            var beneficio = await _service.CriarBeneficioAsync(dto);
            return StatusCode(201, beneficio);
        }

        [HttpPost("vincular")]
        public async Task<IActionResult> Vincular([FromBody] VincularBeneficioDto dto)
        {
            try 
            {
                // Passamos o 'dto' direto
                await _service.VincularBeneficioAsync(dto);
                return Ok(new { mensagem = "Benefício vinculado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var lista = await _service.ListarDisponiveisAsync();
            return Ok(lista);
        }
    }
}