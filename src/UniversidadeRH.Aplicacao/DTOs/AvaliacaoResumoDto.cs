namespace UniversidadeRH.Aplicacao.Dtos
{
    public class AvaliacaoResumoDto
    {
        public Guid Id { get; set; }
        public decimal Nota { get; set; }
        public string Feedback { get; set; } = string.Empty;
        public DateTime DataAvaliacao { get; set; }
    }
}