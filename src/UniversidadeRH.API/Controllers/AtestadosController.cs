using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversidadeRH.Aplicacao.DTOs;
using UniversidadeRH.Aplicacao.Interfaces;

namespace UniversidadeRH.API.Controllers
{
    [Authorize] // Protege o endpoint
    [ApiController]
    [Route("api/atestados")]
    [Tags("05. Saúde e Segurança do Trabalho")]
    public class AtestadosController : ControllerBase
    {
        // 👇 Injeção do Serviço 
        private readonly IAtestadoService _service;

        public AtestadosController(IAtestadoService service)
        {
            _service = service;
        }

        /// <summary>
        /// Registra os dados de um atestado médico (CRM, Dias, Médico).
        /// </summary>
        /// <remarks>
        /// Nota: O upload do arquivo PDF será implementado em uma versão futura.
        /// Por enquanto, registramos apenas os metadados para justificativa de ponto.
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] RegistrarAtestadoDto dto)
        {
            try
            {
                // Chama a lógica de negócio (Valida dias > 0, CRM obrigatório)
                var atestado = await _service.RegistrarAtestadoAsync(dto);
                
                return StatusCode(201, new { 
                    mensagem = "Atestado registrado com sucesso.", 
                    id = atestado.Id,
                    diasAbonados = atestado.DiasAfastamento
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        /// <summary>
        /// Consulta o histórico de atestados de um funcionário.
        /// </summary>
        [HttpGet("funcionario/{id}")]
        public async Task<IActionResult> ObterHistorico(Guid id)
        {
            var lista = await _service.ConsultarHistoricoAsync(id);
            return Ok(lista);
        }
    }
}