using UniversidadeRH.Aplicacao.DTOs;
using UniversidadeRH.Aplicacao.Interfaces;
using UniversidadeRH.Dominio.Entidades;
using UniversidadeRH.Dominio.Enums;
using UniversidadeRH.Dominio.Interfaces;

namespace UniversidadeRH.Aplicacao.Servicos
{
    public class CarreiraService : ICarreiraService
    {
        // Precisamos do Repositório que traz as coleções (Include)
        // No seu código anterior você chamou de _repo.ObterPorIdComAtividadesAsync
        // Vamos assumir que ICarreiraRepositorio ou IFuncionarioRepositorio tenha esses métodos.
        // Vou usar IFuncionarioRepositorio pois é quem gerencia o Funcionario completo.
        private readonly IFuncionarioRepositorio _repo;
        private readonly ICarreiraRepositorio _carreiraRepo; // Para salvar o histórico separado

        public CarreiraService(IFuncionarioRepositorio repo, ICarreiraRepositorio carreiraRepo)
        {
            _repo = repo;
            _carreiraRepo = carreiraRepo;
        }

        public async Task RegistrarAtividadeAcademicaAsync(RegistrarAtividadeAcademicaDto dto)
        {
            var funcionario = await _repo.ObterPorIdComAtividadesAsync(dto.FuncionarioId);
            if (funcionario == null) throw new Exception("Funcionário não encontrado.");

            var tipo = (TipoAtividade)dto.TipoId;
            var atividade = new AtividadeAcademica(dto.FuncionarioId, dto.Descricao, tipo, dto.HorasSemanais);

            // A entidade valida se pode adicionar (ex: limite 40h)
            funcionario.AdicionarAtividadeAcademica(atividade);

            await _repo.AtualizarAsync(funcionario);
        }

        public async Task AdicionarPontuacaoAcademicaAsync(RegistrarProducaoAcademicaDto dto)
        {
            var funcionario = await _repo.ObterPorIdAsync(dto.FuncionarioId);
            if (funcionario == null) throw new Exception("Funcionário não encontrado.");

            // A entidade soma pontos e verifica se vira Titular
            funcionario.AdicionarPontosCarreira(dto.Pontos, dto.Descricao);

            await _repo.AtualizarAsync(funcionario);
        }

        public async Task ProcessarPromocaoTecnicaAsync(ProcessarPromocaoTecnicaDto dto)
        {
            // Precisamos das avaliações para calcular média
            var funcionario = await _repo.ObterPorIdComAvaliacoesAsync(dto.FuncionarioId);
            if (funcionario == null) throw new Exception("Funcionário não encontrado.");

            // Guarda o nível antigo para o histórico
            var nivelAntigo = funcionario.NivelTecnico.ToString();

            // A entidade verifica tempo (2 anos) e nota (>7)
            // Se falhar, ela lança DomainException e o Controller devolve 400 Bad Request
            funcionario.TentarPromocao();

            var novoNivel = funcionario.NivelTecnico.ToString();

            // Se mudou de nível, salvamos o histórico
            if (nivelAntigo != novoNivel)
            {
                var historico = new HistoricoPromocao(dto.FuncionarioId, nivelAntigo!, novoNivel!, dto.Motivo);
                await _carreiraRepo.RegistrarPromocaoAsync(historico);
            }

            await _repo.AtualizarAsync(funcionario);
        }
    }
}