using System.ComponentModel.DataAnnotations;

namespace UniversidadeRH.Aplicacao.DTOs
{
    public class RegistrarAvaliacaoDto
    {
        [Required] public Guid FuncionarioId { get; set; }
        [Required] public decimal Nota { get; set; }
        public string Feedback { get; set; } = string.Empty;
    }
    
    // DTO auxiliar para criar cursos (opcional, mas útil para popular o banco)
    public class CriarCursoDto 
    {
        public string Descricao { get; set; } = string.Empty;
        public int Nivel { get; set; }
        public int TipoFuncionario { get; set; }
        public int CargaHoraria { get; set; }
    }
}