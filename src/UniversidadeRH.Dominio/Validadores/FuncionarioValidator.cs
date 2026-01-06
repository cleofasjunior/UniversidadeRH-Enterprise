using FluentValidation;
using UniversidadeRH.Dominio.Entidades;
using UniversidadeRH.Dominio.Enums;

namespace UniversidadeRH.Dominio.Validadores;

public class FuncionarioValidator : AbstractValidator<Funcionario>
{
    public FuncionarioValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .Length(3, 100).WithMessage("O nome deve ter entre 3 e 100 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("E-mail inválido.");

        RuleFor(x => x.Departamento)
            .NotEmpty().WithMessage("O departamento é obrigatório.");

        // A REGRA DE NEGÓCIO CRÍTICA
        RuleFor(x => x.LinkLattes)
            .NotEmpty()
            .When(x => x.Tipo == TipoFuncionario.Professor)
            .WithMessage("O Link do Lattes é obrigatório para Professores.");
            
        // Regra Extra: Validação simples de URL se o campo não for vazio
        RuleFor(x => x.LinkLattes)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .When(x => !string.IsNullOrEmpty(x.LinkLattes))
            .WithMessage("O Link do Lattes deve ser uma URL válida (ex: http://lattes.cnpq.br/...).");
    }
}