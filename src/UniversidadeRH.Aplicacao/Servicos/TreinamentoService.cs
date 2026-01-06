using UniversidadeRH.Aplicacao.DTOs;
using UniversidadeRH.Aplicacao.Interfaces;
using UniversidadeRH.Dominio.Entidades;
using UniversidadeRH.Dominio.Interfaces;
using UniversidadeRH.Dominio.Enums; 

namespace UniversidadeRH.Aplicacao.Servicos
{
    public class TreinamentoService : ITreinamentoService
    {
        private readonly ITreinamentoRepositorio _treinamentoRepo;
        private readonly IFuncionarioRepositorio _funcionarioRepo;

        public TreinamentoService(ITreinamentoRepositorio treinamentoRepo, IFuncionarioRepositorio funcionarioRepo)
        {
            _treinamentoRepo = treinamentoRepo;
            _funcionarioRepo = funcionarioRepo;
        }

        public async Task CriarCursoAsync(string descricao, int nivel, int tipoFuncionario, int cargaHoraria)
        {
            var curso = new Treinamento(
                descricao, 
                (NivelTreinamento)nivel,          
                (TipoFuncionario)tipoFuncionario, 
                cargaHoraria                      
            );
            
            await _treinamentoRepo.AdicionarAsync(curso);
        }

        public async Task<List<Treinamento>> ListarTodosCursosAsync()
        {
            return await _treinamentoRepo.ObterTodosAsync();
        }

        public async Task<Funcionario> RegistrarAvaliacao(RegistrarAvaliacaoDto dto)
        {
            var funcionario = await _funcionarioRepo.ObterPorIdAsync(dto.FuncionarioId);
            
            if (funcionario == null) throw new Exception("Funcionário não encontrado.");

            var novaAvaliacao = new AvaliacaoDesempenho(dto.FuncionarioId, dto.Nota, dto.Feedback);
            
            // Apenas adiciona a avaliação. NADA DE SUGESTÃO AUTOMÁTICA.
            funcionario.AdicionarAvaliacao(novaAvaliacao);

           
            return funcionario;
        }

        public async Task<List<AvaliacaoResumoDto>> ListarAvaliacoesPorFuncionarioAsync(Guid funcionarioId)
        {
            var funcionario = await _funcionarioRepo.ObterPorIdAsync(funcionarioId);
            
            if (funcionario == null || !funcionario.Avaliacoes.Any()) 
                return new List<AvaliacaoResumoDto>();

            var listaDtos = funcionario.Avaliacoes
                .OrderByDescending(a => a.DataAvaliacao)
                .Select(a => new AvaliacaoResumoDto
                {
                    Id = a.Id,
                    Nota = a.Nota,
                    Feedback = a.Feedback,
                    DataAvaliacao = a.DataAvaliacao
                })
                .ToList();

            return listaDtos;
        }
    } 
}