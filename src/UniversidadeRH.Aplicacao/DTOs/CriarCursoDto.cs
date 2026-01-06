using UniversidadeRH.Dominio.Enums; 

namespace UniversidadeRH.Aplicacao.Dtos
{
    public class CriarCursoDto
    {
        public string Descricao { get; set; } = string.Empty;
        public int CargaHoraria { get; set; }
        public NivelTreinamento Nivel { get; set; } // Ou int, se preferir
        public TipoFuncionario TipoFuncionario { get; set; } // Ou int
    }
}