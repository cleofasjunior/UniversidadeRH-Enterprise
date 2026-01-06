using System.ComponentModel.DataAnnotations;

namespace UniversidadeRH.Aplicacao.DTOs
{
    // 1. INPUT: Dados para registrar
    public class RegistrarFuncionarioDto
    {
        [Required] public string Nome { get; set; } = string.Empty;
        
        [Required] public string Email { get; set; } = string.Empty;
        
        [Required] public string Cpf { get; set; } = string.Empty; 

        [Required] public string Departamento { get; set; } = string.Empty;
        
        // 1 = Docente, 2 = Técnico
        [Required] public int TipoFuncionario { get; set; } 
        
        public string? LinkLattes { get; set; }

        // === NOVOS CAMPOS (Para Correção dos Testes de Carreira) ===
        
        // 0=Nenhum, 1=20h, 2=40h, 3=Dedicação Exclusiva
        public int Regime { get; set; } 

        // Útil para simular funcionários antigos e testar promoção
        public DateTime? DataAdmissao { get; set; } 
    }

    // 2. OUTPUT: Dados para exibir na tela
    public record FuncionarioViewModel(
        Guid Id,
        string Nome,
        string Email,
        string Departamento,
        string Tipo,
        string? LinkLattes,
        DateTime DataAdmissao
    );
}