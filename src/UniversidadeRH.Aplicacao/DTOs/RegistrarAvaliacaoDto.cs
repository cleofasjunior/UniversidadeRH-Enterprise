using System;
using System.ComponentModel.DataAnnotations;

namespace UniversidadeRH.Aplicacao.DTOs; 

public class RegistrarAvaliacaoDto
{
    [Required(ErrorMessage = "O ID do funcionário é obrigatório.")]
    public Guid FuncionarioId { get; set; }

    [Range(0, 10, ErrorMessage = "A nota deve ser entre 0 e 10.")]
    public decimal Nota { get; set; } // <--- Alterado de double para decimal (para bater com o teste)

    // Mantivemos o Feedback para compatibilidade, mas o teste usa Comentarios
    [StringLength(500, ErrorMessage = "O feedback deve ter no máximo 500 caracteres.")]
    public string Feedback { get; set; } = string.Empty;

    // Propriedade usada no Teste de Integração
    public string? Comentarios { get; set; }
}