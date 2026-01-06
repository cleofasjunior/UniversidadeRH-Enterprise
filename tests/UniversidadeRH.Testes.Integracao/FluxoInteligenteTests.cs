using FluentAssertions;
using Moq;
using UniversidadeRH.Aplicacao.DTOs;
using UniversidadeRH.Aplicacao.Servicos;
using UniversidadeRH.Dominio.Entidades;
using UniversidadeRH.Dominio.Enums;
using UniversidadeRH.Dominio.Interfaces;
using Xunit;

namespace UniversidadeRH.Testes.Integracao;

public class FluxoInteligenteTests
{
    [Fact(DisplayName = "LÓGICA: Deve Sugerir Curso se a Nota for Baixa")]
    public async Task Deve_Sugerir_Curso_Automaticamente_Apos_Avaliacao_Ruim()
    {
        // =============================================================
        // 1. ARRANGE (Preparação)
        // =============================================================
        
        var mockFuncRepo = new Mock<IFuncionarioRepositorio>();
        var mockTreinoRepo = new Mock<ITreinamentoRepositorio>();

        // 1.1 Criar o Funcionário
        var funcionario = new Funcionario(
            "Tony Stark", 
            "ironman@vingadores.com", 
            "Engenharia", 
            (TipoFuncionario)1, // Técnico
            null
        );
        var funcionarioId = funcionario.Id;

        // 1.2 Criar um Curso para o catálogo
        var cursoParaSugerir = new Treinamento(
            "Controle de Raiva",    // Nome
            (NivelTreinamento)1,    // Nível (Básico)
            (TipoFuncionario)1,     // Público Alvo (Técnico)
            40                      // Carga Horária
        );

        var listaDeCursos = new List<Treinamento> { cursoParaSugerir };

        // --- CONFIGURAÇÃO DOS MOCKS ---

        // Config 1: Repositório de Treinamentos
        // 👇👇👇 AQUI ESTAVA O ERRO! Agora usamos o nome certo: ObterCatalogoAsync 👇👇👇
        mockTreinoRepo.Setup(repo => repo.ObterCatalogoAsync()) 
            .ReturnsAsync(listaDeCursos);

        // Config 2: Repositório de Funcionários (Configuramos todas as buscas possíveis)
        mockFuncRepo.Setup(repo => repo.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(funcionario);
        mockFuncRepo.Setup(repo => repo.ObterPorIdComTreinamentosAsync(It.IsAny<Guid>())).ReturnsAsync(funcionario);
        mockFuncRepo.Setup(repo => repo.ObterPorIdComAvaliacoesAsync(It.IsAny<Guid>())).ReturnsAsync(funcionario);

        // Instancia o Serviço
        var service = new TreinamentoService(mockTreinoRepo.Object, mockFuncRepo.Object);

        var dtoAvaliacao = new RegistrarAvaliacaoDto
        {
            FuncionarioId = funcionarioId,
            Nota = 4.0m, // Nota que ativa a regra (< 5)
            Feedback = "Precisa melhorar a paciência"
        };

        // =============================================================
        // 2. ACT (Ação)
        // =============================================================
        
        await service.RegistrarAvaliacaoDesempenhoAsync(dtoAvaliacao);

        // =============================================================
        // 3. ASSERT (Validação)
        // =============================================================

        // Verifica se o curso foi adicionado
        funcionario.Treinamentos.Should().HaveCount(1);
        
        // Verifica o motivo
        funcionario.Treinamentos.First().MotivoIndicacao.Should().Contain("Desempenho Crítico");

        // Verifica se tentou salvar
        mockFuncRepo.Verify(repo => repo.AtualizarAsync(funcionario), Times.Once);
    }
}