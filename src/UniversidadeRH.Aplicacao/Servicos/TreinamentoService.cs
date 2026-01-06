using UniversidadeRH.Aplicacao.Dtos;
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
            return await _treinamentoRepo.ListarTodosAsync();
        }

        public async Task RegistrarAvaliacaoDesempenhoAsync(RegistrarAvaliacaoDto dto)
        {
            var funcionario = await _funcionarioRepo.ObterPorIdComAvaliacoesAsync(dto.FuncionarioId);
            
            if (funcionario == null) throw new Exception("Funcionário não encontrado.");

            var novaAvaliacao = new AvaliacaoDesempenho(dto.FuncionarioId, (decimal)dto.Nota, dto.Feedback);

            funcionario.Avaliacoes?.Add(novaAvaliacao);

            await _funcionarioRepo.AtualizarAsync(funcionario);
        }

        public async Task<List<AvaliacaoResumoDto>> ListarAvaliacoesPorFuncionarioAsync(Guid funcionarioId)
        {
            // 1. Usa o Repositório para buscar funcionário JÁ COM as avaliações (Include)
            var funcionario = await _funcionarioRepo.ObterPorIdComAvaliacoesAsync(funcionarioId);
            
            if (funcionario == null || funcionario.Avaliacoes == null) 
                return new List<AvaliacaoResumoDto>();

            // 2. Transforma (Mapeia) a Entidade para o DTO
            var listaDtos = funcionario.Avaliacoes
                .OrderByDescending(a => a.DataAvaliacao) // Mostra as mais recentes primeiro
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
    } // Fim da classe
}