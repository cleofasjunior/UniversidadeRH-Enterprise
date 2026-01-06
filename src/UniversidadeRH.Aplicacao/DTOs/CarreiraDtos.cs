using System.ComponentModel.DataAnnotations;

namespace UniversidadeRH.Aplicacao.DTOs
{
    // Docentes
    public class RegistrarAtividadeAcademicaDto
    {
        [Required] public Guid FuncionarioId { get; set; }
        [Required] public string Descricao { get; set; } = string.Empty;
        [Required] public int TipoId { get; set; } // 1=Ensino, 2=Pesquisa...
        [Required] public int HorasSemanais { get; set; }
    }

    // Docentes
    public class RegistrarProducaoAcademicaDto
    {
        [Required] public Guid FuncionarioId { get; set; }
        [Required] public string Descricao { get; set; } = string.Empty;
        [Required] public int Pontos { get; set; }
    }

    // Técnicos
    public class ProcessarPromocaoTecnicaDto
    {
        [Required] public Guid FuncionarioId { get; set; }
        // O nível é calculado automaticamente pelo domínio!
        // Apenas o motivo é necessário para o histórico (opcional)
        public string Motivo { get; set; } = "Progressão Automática por Mérito e Tempo";
    }
}