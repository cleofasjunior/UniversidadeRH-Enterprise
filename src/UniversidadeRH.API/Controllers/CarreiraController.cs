using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversidadeRH.Aplicacao.DTOs;
using UniversidadeRH.Aplicacao.Interfaces;

namespace UniversidadeRH.API.Controllers
{
    [Authorize] // Protege as rotas (precisa de Token)
    [ApiController]
    [Route("api/carreira")]
    [Tags("04. Gestão de Carreira e Cargos")]
    public class CarreiraController : ControllerBase
    {
        // 👇 Injeção de Dependência do Serviço
        private readonly ICarreiraService _service;

        public CarreiraController(ICarreiraService service)
        {
            _service = service;
        }

        // --- DOCENTES ---

        /// <summary>
        /// [Docente] Registra uma atividade (Aula, Pesquisa) validando a carga horária do contrato.
        /// </summary>
        [HttpPost("docente/atividade")]
        public async Task<IActionResult> AddAtividade([FromBody] RegistrarAtividadeAcademicaDto dto)
        {
            try
            {
                // Chama o serviço real
                await _service.RegistrarAtividadeAcademicaAsync(dto);
                return Ok(new { mensagem = "Atividade registrada e carga horária atualizada." });
            }
            catch (Exception ex)
            {
                // Retorna erro 400 se estourar as 40h ou funcionário não existir
                return BadRequest(new { erro = ex.Message });
            }
        }

        /// <summary>
        /// [Docente] Adiciona pontos de produção e verifica promoção automática.
        /// </summary>
        [HttpPost("docente/pontuacao")]
        public async Task<IActionResult> AddPontuacao([FromBody] RegistrarProducaoAcademicaDto dto)
        {
            try
            {
                await _service.AdicionarPontuacaoAcademicaAsync(dto);
                return Ok(new { mensagem = "Pontos computados. Nível de carreira verificado." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        // --- TÉCNICOS ---

        /// <summary>
        /// [Técnico] Processa a progressão funcional baseada no PCCR (Tempo + Avaliação).
        /// </summary>
        [HttpPost("tecnico/processar-promocao")]
        public async Task<IActionResult> ProcessarPromocao([FromBody] ProcessarPromocaoTecnicaDto dto)
        {
            try
            {
                // O serviço vai validar os 2 anos e a média 7.0
                await _service.ProcessarPromocaoTecnicaAsync(dto);
                return Ok(new { mensagem = "Análise de promoção concluída com sucesso." });
            }
            catch (Exception ex)
            {
                // Se der erro (ex: "Não tem 2 anos ainda"), devolve a mensagem clara
                return BadRequest(new { erro = ex.Message });
            }
        }
    }
}