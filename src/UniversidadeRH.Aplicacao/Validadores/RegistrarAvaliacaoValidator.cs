using FluentValidation;
using UniversidadeRH.Aplicacao.DTOs;

namespace UniversidadeRH.Aplicacao.Validadores
{
    public class RegistrarAvaliacaoValidator : AbstractValidator<RegistrarAvaliacaoDto>
    {
        public RegistrarAvaliacaoValidator()
        {
            RuleFor(x => x.FuncionarioId)
                .NotEmpty().WithMessage("O ID do funcionário é obrigatório.");

            RuleFor(x => x.Nota)
                .InclusiveBetween(0, 10).WithMessage("A nota deve ser entre 0 e 10.");

            RuleFor(x => x.Feedback)
                .NotEmpty().WithMessage("O feedback é obrigatório.")
                .MaximumLength(500).WithMessage("O feedback não pode exceder 500 caracteres.");
        }
    }
}