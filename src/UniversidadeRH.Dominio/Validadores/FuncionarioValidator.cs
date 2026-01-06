using FluentValidation;
using UniversidadeRH.Dominio.Entidades;
using UniversidadeRH.Dominio.Enums;

namespace UniversidadeRH.Dominio.Validadores;

public class FuncionarioValidator : AbstractValidator<Funcionario>
{
    public FuncionarioValidator()
    {
        // 1. Nome
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .Length(3, 100).WithMessage("O nome deve ter entre 3 e 100 caracteres.");

        // 2. Email
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O email é obrigatório.")
            .EmailAddress().WithMessage("O email fornecido não é válido.");

        // 3. Departamento
        RuleFor(x => x.Departamento)
            .NotEmpty().WithMessage("O departamento é obrigatório.");

        // 4. CPF
        RuleFor(x => x.Cpf)
            .NotEmpty().WithMessage("CPF é obrigatório.");

        // 5. Lattes (Regra condicional corrigida para usar .Tipo)
        RuleFor(x => x.LinkLattes)
            .NotEmpty()
            .When(x => x.Tipo == TipoFuncionario.Professor)
            .WithMessage("Lattes é obrigatório para professores.");
    }
}