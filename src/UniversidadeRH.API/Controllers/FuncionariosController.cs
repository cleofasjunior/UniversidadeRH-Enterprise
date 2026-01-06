using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversidadeRH.Aplicacao.DTOs;
using UniversidadeRH.Aplicacao.Interfaces;

namespace UniversidadeRH.API.Controllers
{
    [Authorize] // Exige que o usuário esteja logado (Token JWT)
    [Route("api/[controller]")] // URL fica: /api/funcionarios
    [ApiController]
    [Tags("01. Gestão de Funcionários")] // Tag numerada para ordenação no Swagger
    public class FuncionariosController : ControllerBase
    {
        private readonly IFuncionarioService _service;

        public FuncionariosController(IFuncionarioService service)
        {
            _service = service;
        }

        /// <summary>
        /// Registra um novo funcionário na base de dados.
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     POST /api/funcionarios
        ///     {
        ///        "nome": "Dr. Estranho",
        ///        "email": "estranho@uni.edu",
        ///        "departamento": "Medicina",
        ///        "tipoFuncionario": 1, 
        ///        "linkLattes": "http://lattes.cnpq.br/..."
        ///     }
        ///
        /// </remarks>
        /// <param name="dto">Dados do funcionário para cadastro.</param>
        /// <returns>O objeto criado com seu ID.</returns>
        /// <response code="201">Sucesso: Funcionário criado.</response>
        /// <response code="400">Erro: Falha na validação de negócio.</response>
        /// <response code="401">Erro: Usuário não autenticado.</response>
        [HttpPost]
        [ProducesResponseType(typeof(FuncionarioViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // 👇 Mudamos para RegistrarFuncionarioDto
        public async Task<IActionResult> Registrar([FromBody] RegistrarFuncionarioDto dto)
        {
            try
            {
                // 👇 Mudamos para o nome correto do método no serviço
                var resultado = await _service.RegistrarFuncionarioAsync(dto);
                
                // Gera o header 'Location' na resposta HTTP (Padrão REST)
                return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Id }, resultado);
            }
            catch (ArgumentException ex)
            {
                // Captura erros de validação de negócio (Ex: Email já existe)
                return BadRequest(new { 
                    mensagem = "Erro de Validação", 
                    erros = ex.Message.Split(';') 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    mensagem = "Erro interno no servidor", 
                    detalhe = ex.Message 
                });
            }
        }

        /// <summary>
        /// Busca um funcionário pelo seu ID único.
        /// </summary>
        /// <param name="id">O ID do funcionário (GUID).</param>
        /// <returns>Os detalhes do funcionário.</returns>
        /// <response code="200">Sucesso: Retorna o funcionário.</response>
        /// <response code="404">Erro: Funcionário não encontrado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FuncionarioViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            try 
            {
                var funcionario = await _service.ObterPorIdAsync(id);

                if (funcionario == null)
                {
                    return NotFound(new { mensagem = "Funcionário não encontrado." });
                }

                return Ok(funcionario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao buscar funcionário", detalhe = ex.Message });
            }
        }

        /// <summary>
        /// Lista todos os funcionários cadastrados.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            // 👇Agora chamamos o serviço real em vez do exemplo fixo
            var lista = await _service.ListarTodosAsync();
            return Ok(lista);
        }
    }
}