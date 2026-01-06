using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversidadeRH.Aplicacao.Interfaces;

namespace UniversidadeRH.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [Tags("03. Módulo de Férias")]
    public class FeriasController : ControllerBase
    {
        private readonly IFeriasService _service;

        public FeriasController(IFeriasService service)
        {
            _service = service;
        }

        /// <summary>
        /// Solicita um período de férias para o funcionário.
        /// </summary>
        [HttpPost("solicitar")]
        public async Task<IActionResult> Solicitar([FromBody] SolicitarFeriasDto dto)
        {
            try
            {
                var ferias = await _service.SolicitarFeriasAsync(dto.FuncionarioId, dto.DataInicio, dto.DataFim);
                return StatusCode(201, ferias);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }

        /// <summary>
        /// Lista o histórico de férias de um funcionário.
        /// </summary>
        [HttpGet("funcionario/{id}")]
        public async Task<IActionResult> ListarPorFuncionario(Guid id)
        {
            var lista = await _service.ConsultarFeriasDoFuncionarioAsync(id);
            return Ok(lista);
        }
    }

    // DTO local para agilizar (pode mover para pasta DTOs depois)
    public class SolicitarFeriasDto
    {
        public Guid FuncionarioId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}