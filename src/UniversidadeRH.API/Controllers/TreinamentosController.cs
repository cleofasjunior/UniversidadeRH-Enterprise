using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversidadeRH.Aplicacao.Dtos; 
using UniversidadeRH.Aplicacao.Interfaces;
using UniversidadeRH.Dominio.Enums; 

namespace UniversidadeRH.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [Tags("06. Desenvolvimento e Treinamento (LMS)")]
    public class TreinamentosController : ControllerBase
    {
        private readonly ITreinamentoService _service;

        public TreinamentosController(ITreinamentoService service)
        {
            _service = service;
        }

        // --- PASSO 1: CRIAR CURSO (CATÁLOGO) ---
        [HttpPost("criar-curso")]
        public async Task<IActionResult> CriarCurso([FromBody] CriarCursoDto dto)
        {
           
            await _service.CriarCursoAsync(
                dto.Descricao, 
                (int)dto.Nivel,            
                (int)dto.TipoFuncionario,  
                dto.CargaHoraria
            );

            return StatusCode(201, new { mensagem = "Curso adicionado ao catálogo com sucesso." });
        }

        // --- PASSO 2: LISTAR CURSOS ---
        [HttpGet]
        public async Task<IActionResult> ListarCursos()
        {
            var cursos = await _service.ListarTodosCursosAsync();
            return Ok(cursos);
        }

        // --- PASSO 3: AVALIAR DESEMPENHO ---
        [HttpPost("avaliacao-desempenho")]
        public async Task<IActionResult> RegistrarAvaliacao(
            [FromBody] RegistrarAvaliacaoDto dto,
            [FromServices] IValidator<RegistrarAvaliacaoDto> validator)
        {
            // Validação (Blindagem)
            var resultadoValidacao = await validator.ValidateAsync(dto);

            if (!resultadoValidacao.IsValid)
            {
                return BadRequest(resultadoValidacao.Errors);
            }

            // Execução da Lógica Simplificada
            await _service.RegistrarAvaliacaoDesempenhoAsync(dto);
            
            return Ok(new { mensagem = "Avaliação registrada com sucesso." });
        }

        // --- PASSO 4: LISTAR AVALIAÇÕES ---
        /// <summary>
        /// Lista o histórico de avaliações de desempenho de um funcionário.
        /// </summary>
        [HttpGet("avaliacoes/{funcionarioId}")]
        public async Task<IActionResult> ListarAvaliacoes(Guid funcionarioId)
        {
            var resultado = await _service.ListarAvaliacoesPorFuncionarioAsync(funcionarioId);
            
            // Retorna 200 OK com a lista (mesmo que vazia)
            return Ok(resultado);
        }

    } // Fim da classe
}