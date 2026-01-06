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

    [Theory(DisplayName = "Erro: Nome não pode ser vazio ou nulo")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")] 
    public void Deve_Falhar_Quando_Nome_Invalido(string? nomeInvalido)
    {
        // ORDEM DO CONSTRUTOR: Nome, Email, Departamento, CPF, Tipo, Lattes
        // Usando argumentos posicionais para evitar erro de nome de parâmetro
        var funcionario = new Funcionario(
            nomeInvalido!, 
            "teste@email.com", 
            "TI",
            "123.456.789-00", 
            TipoFuncionario.TecnicoAdministrativo, 
            null
        );

        var resultado = _validator.Validate(funcionario);
        resultado.IsValid.Should().BeFalse();
    }

    [Theory(DisplayName = "Erro: Departamento não pode ser vazio")]
    [InlineData("")]
    [InlineData(null)]
    public void Deve_Falhar_Quando_Departamento_Invalido(string? departamentoInvalido)
    {
        var funcionario = new Funcionario(
            "Teste", 
            "teste@email.com", 
            departamentoInvalido!, 
            "123.456.789-00", 
            TipoFuncionario.TecnicoAdministrativo, 
            null
        );

        var resultado = _validator.Validate(funcionario);
        resultado.IsValid.Should().BeFalse();
    }

    [Theory(DisplayName = "Erro: Email inválido")]
    [InlineData("email_sem_arroba.com")]
    [InlineData("usuario@")]
    [InlineData("@dominio.com")]
    [InlineData("")]
    [InlineData(null)]
    public void Deve_Falhar_Quando_Email_Invalido(string? emailInvalido)
    {
        var funcionario = new Funcionario(
            "Teste", 
            emailInvalido!, 
            "Dept", 
            "123.456.789-00", 
            TipoFuncionario.TecnicoAdministrativo, 
            null
        );

        var resultado = _validator.Validate(funcionario);
        resultado.IsValid.Should().BeFalse();
    }

    [Fact(DisplayName = "Erro: Professor DEVE ter Link Lattes")]
    public void Deve_Falhar_Quando_Professor_Nao_Tem_Lattes()
    {
        var funcionario = new Funcionario(
            "Professor Xavier",
            "xavier@xmen.edu",
            "Mutantes",
            "999.888.777-66", 
            TipoFuncionario.Professor, 
            "" // Lattes vazio (Erro)
        );

        var resultado = _validator.Validate(funcionario);
        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(x => x.ErrorMessage.Contains("Lattes é obrigatório"));
    }

    [Fact(DisplayName = "Sucesso: Técnico PODE ficar sem Lattes")]
    public void Deve_Passar_Quando_Tecnico_Nao_Tem_Lattes()
    {
        var funcionario = new Funcionario(
            "Tony Stark",
            "tony@stark.com",
            "Engenharia",
            "111.222.333-44",
            TipoFuncionario.TecnicoAdministrativo,
            null
        );

        var resultado = _validator.Validate(funcionario);
        resultado.IsValid.Should().BeTrue();
    }

    [Fact(DisplayName = "Sucesso: Funcionário Completo e Válido")]
    public void Deve_Passar_Quando_Funcionario_Estiver_Valido()
    {
        var funcionario = new Funcionario(
            "Funcionario Modelo",
            "modelo@rh.com",
            "RH",
            "555.666.777-88",
            TipoFuncionario.Professor,
            "http://lattes.cnpq.br/123456" 
        );

        var resultado = _validator.Validate(funcionario);
        resultado.IsValid.Should().BeTrue();
    }
}