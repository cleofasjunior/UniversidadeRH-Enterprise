using FluentAssertions;
using UniversidadeRH.Dominio.Entidades;
using UniversidadeRH.Dominio.Enums;
using UniversidadeRH.Dominio.Validadores;
using Xunit;

namespace UniversidadeRH.Testes.Unidade.Dominio;

public class FuncionarioValidatorTests
{
    private readonly FuncionarioValidator _validator;

    public FuncionarioValidatorTests()
    {
        _validator = new FuncionarioValidator();
    }

    // --- TESTES DE CAMPOS OBRIGATÓRIOS (SANITIZAÇÃO) ---

    [Theory(DisplayName = "Erro: Nome não pode ser vazio ou nulo")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")] 
   
    public void Deve_Falhar_Quando_Nome_Invalido(string? nomeInvalido)
    {
        // Arrange
        
        var funcionario = new Funcionario(
            nome: nomeInvalido!, 
            email: "teste@email.com", 
            departamento: "TI", 
            tipo: TipoFuncionario.TecnicoAdministrativo, 
            linkLattes: null
        );

        // Act
        var resultado = _validator.Validate(funcionario);

        // Assert
        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(x => x.PropertyName == "Nome");
    }

    [Theory(DisplayName = "Erro: Departamento não pode ser vazio")]
    [InlineData("")]
    [InlineData(null)]
    
    public void Deve_Falhar_Quando_Departamento_Invalido(string? departamentoInvalido)
    {
        // Arrange
        var funcionario = new Funcionario("Teste", "teste@email.com", departamentoInvalido!, TipoFuncionario.TecnicoAdministrativo, null);

        // Act
        var resultado = _validator.Validate(funcionario);

        // Assert
        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(x => x.PropertyName == "Departamento");
    }

    [Theory(DisplayName = "Erro: Email inválido ou vazio deve falhar")]
    [InlineData("email_sem_arroba.com")]
    [InlineData("usuario@")]
    [InlineData("@dominio.com")]
    [InlineData("")]
    [InlineData(null)]
    
    public void Deve_Falhar_Quando_Email_Invalido(string? emailInvalido)
    {
        // Arrange
        var funcionario = new Funcionario("Teste", emailInvalido!, "Dept", TipoFuncionario.TecnicoAdministrativo, null);

        // Act
        var resultado = _validator.Validate(funcionario);

        // Assert
        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    // --- TESTES DE REGRA DE NEGÓCIO (LÓGICA DO RH) ---

    [Fact(DisplayName = "Erro: Professor DEVE ter Link Lattes")]
    public void Deve_Falhar_Quando_Professor_Nao_Tem_Lattes()
    {
        // Arrange
        var funcionario = new Funcionario(
            nome: "Professor Xavier",
            email: "xavier@xmen.edu",
            departamento: "Mutantes",
            tipo: TipoFuncionario.Professor, 
            linkLattes: "" 
        );

        // Act
        var resultado = _validator.Validate(funcionario);

        // Assert
        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(x => x.ErrorMessage.Contains("Lattes é obrigatório"));
    }

    [Fact(DisplayName = "Sucesso: Técnico PODE ficar sem Lattes")]
    public void Deve_Passar_Quando_Tecnico_Nao_Tem_Lattes()
    {
        // Arrange
        var funcionario = new Funcionario(
            nome: "Tony Stark",
            email: "tony@stark.com",
            departamento: "Engenharia",
            tipo: TipoFuncionario.TecnicoAdministrativo,
            linkLattes: null 
        );

        // Act
        var resultado = _validator.Validate(funcionario);

        // Assert
        resultado.IsValid.Should().BeTrue();
    }

    [Fact(DisplayName = "Sucesso: Funcionário Completo e Válido")]
    public void Deve_Passar_Quando_Funcionario_Estiver_Valido()
    {
        // Arrange
        var funcionario = new Funcionario(
            nome: "Funcionario Modelo",
            email: "modelo@rh.com",
            departamento: "RH",
            tipo: TipoFuncionario.Professor,
            linkLattes: "http://lattes.cnpq.br/123456" 
        );

        // Act
        var resultado = _validator.Validate(funcionario);

        // Assert
        resultado.IsValid.Should().BeTrue();
    }
}