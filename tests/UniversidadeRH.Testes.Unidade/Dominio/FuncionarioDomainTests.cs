using FluentAssertions;
using UniversidadeRH.Dominio.Entidades;
using UniversidadeRH.Dominio.Enums;
using Xunit;

namespace UniversidadeRH.Testes.Unidade.Dominio;

public class FuncionarioDomainTests
{
    [Fact(DisplayName = "LMS: Nota baixa deve sugerir treinamento automaticamente")]
    public void Deve_Adicionar_Treinamento_Quando_Nota_For_Baixa()
    {
        // Arrange (Cenário)
        var funcionario = new Funcionario("Joao", "joao@teste.com", "TI", TipoFuncionario.TecnicoAdministrativo, null);
        
        // Simulamos o Catálogo de Cursos vindo do Banco
        var catalogo = new List<Treinamento>
        {
            new Treinamento("Curso Básico", NivelTreinamento.Basico, TipoFuncionario.TecnicoAdministrativo, 20),
            new Treinamento("Curso Avançado", NivelTreinamento.Avancado, TipoFuncionario.TecnicoAdministrativo, 40)
        };

        // Act (Ação: Damos uma nota ruim < 5.0)
        funcionario.RegistrarAvaliacao(4.0m, "Precisa melhorar", catalogo);

        // Assert (Verificação)
        funcionario.Avaliacoes.Should().HaveCount(1); // A avaliação foi salva
        funcionario.Treinamentos.Should().HaveCount(1); // O curso foi sugerido!
        
        var treinoSugerido = funcionario.Treinamentos.First();
        treinoSugerido.Concluido.Should().BeFalse(); // Deve estar pendente
        
        // Ajustamos a frase para bater com a lógica real do sistema
        treinoSugerido.MotivoIndicacao.Should().Contain("Desempenho Crítico");
    }

    [Fact(DisplayName = "LMS: Nota alta NÃO deve sugerir treinamento")]
    public void Nao_Deve_Adicionar_Treinamento_Quando_Nota_For_Alta()
    {
        // Arrange
        var funcionario = new Funcionario("Maria", "maria@teste.com", "RH", TipoFuncionario.TecnicoAdministrativo, null);
        var catalogo = new List<Treinamento>
        {
            new Treinamento("Gestão", NivelTreinamento.Basico, TipoFuncionario.TecnicoAdministrativo, 10)
        };

        // Act (Nota Boa > 5.0)
        funcionario.RegistrarAvaliacao(9.5m, "Excelente trabalho", catalogo);

        // Assert
        funcionario.Avaliacoes.Should().HaveCount(1);
        funcionario.Treinamentos.Should().BeEmpty(); // NENHUM curso sugerido
    }
}