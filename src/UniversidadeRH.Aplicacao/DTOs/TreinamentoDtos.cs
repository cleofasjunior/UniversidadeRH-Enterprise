using System.ComponentModel.DataAnnotations;

namespace UniversidadeRH.Aplicacao.DTOs
{
    
    public class CriarCursoDto 
    {
        public string Descricao { get; set; } = string.Empty;
        public int Nivel { get; set; }
        public int TipoFuncionario { get; set; }
        public int CargaHoraria { get; set; }
    }
}