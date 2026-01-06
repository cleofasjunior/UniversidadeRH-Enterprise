using System.ComponentModel.DataAnnotations;

namespace UniversidadeRH.Aplicacao.DTOs
{
    public class RegistrarAtestadoDto
    {
        [Required] public Guid FuncionarioId { get; set; }
        [Required] public DateTime DataEmissao { get; set; }
        [Required] public int DiasAfastamento { get; set; }
        [Required] public string NomeMedico { get; set; } = string.Empty;
        [Required] public string CRM { get; set; } = string.Empty;
        public string CID { get; set; } = string.Empty;
    }
}