using FluentAssertions;
using UniversidadeRH.Dominio.Entidades;
using UniversidadeRH.Dominio.Excecoes;
using Xunit;

namespace UniversidadeRH.Testes.Unidade.Dominio;

public class AtestadoMedicoTests
{
    [Theory(DisplayName = "Erro: Dias de afastamento inválidos")]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Deve_Lancar_Erro_Quando_Dias_Afastamento_Invalido(int dias)
    {
        // Arrange & Act
        Action action = () => new AtestadoMedico(
            Guid.NewGuid(), 
            DateTime.Now, 
            dias, // Valor inválido
            "Dr. House", 
            "CRM/SP 123456", 
            "Z00.0"
        );

        // Assert
        action.Should().Throw<DomainException>()
            .WithMessage("Atestado deve ter no mínimo 1 dia de afastamento.");
    }

    [Fact(DisplayName = "Erro: CRM Obrigatório")]
    public void Deve_Lancar_Erro_Quando_CRM_Vazio()
    {
        // Arrange & Act
        Action action = () => new AtestadoMedico(
            Guid.NewGuid(), DateTime.Now, 5, "Dr. House", 
            "", // CRM Vazio
            "CID10"
        );

        // Assert
        action.Should().Throw<DomainException>()
            .WithMessage("O CRM do médico é obrigatório.");
    }

    [Fact(DisplayName = "Sucesso: Atestado Válido")]
    public void Deve_Criar_Atestado_Valido()
    {
        // Act
        var atestado = new AtestadoMedico(
            Guid.NewGuid(), DateTime.Now, 3, "Dra. Grey", "12345", "A00"
        );

        // Assert
        atestado.Should().NotBeNull();
        atestado.CID.Should().Be("A00");
    }
}