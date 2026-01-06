using System;
using System.ComponentModel.DataAnnotations; // Para validações básicas

namespace UniversidadeRH.Aplicacao.Dtos
{
    public class RegistrarAvaliacaoDto
    {
        [Required(ErrorMessage = "O ID do funcionário é obrigatório.")]
        public Guid FuncionarioId { get; set; }

        [Range(0, 10, ErrorMessage = "A nota deve ser entre 0 e 10.")]
        public double Nota { get; set; }

        [Required(ErrorMessage = "O feedback/comentário é obrigatório.")]
        [StringLength(500, ErrorMessage = "O feedback deve ter no máximo 500 caracteres.")]
        public string Feedback { get; set; } = string.Empty;
    }
}