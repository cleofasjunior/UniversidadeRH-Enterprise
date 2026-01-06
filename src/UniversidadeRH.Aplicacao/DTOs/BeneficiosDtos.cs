using System.ComponentModel.DataAnnotations;

namespace UniversidadeRH.Aplicacao.DTOs
{
    public class CriarBeneficioDto
    {
        [Required] public string Nome { get; set; } = string.Empty;
        [Required] public decimal Valor { get; set; }
    }

    public class VincularBeneficioDto
    {
        [Required] public Guid FuncionarioId { get; set; }
        [Required] public Guid BeneficioId { get; set; }
    }
}