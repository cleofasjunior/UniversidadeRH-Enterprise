namespace UniversidadeRH.Dominio.Entidades
{
    public class HistoricoPromocao
    {
        public Guid Id { get; private set; }
        public Guid FuncionarioId { get; private set; }
        public DateTime DataPromocao { get; private set; }
        public string NivelAntigo { get; private set; } = string.Empty;
        public string NovoNivel { get; private set; } = string.Empty;
        public string Motivo { get; private set; } = string.Empty;

        // Construtor vazio para o Entity Framework
        protected HistoricoPromocao() { }

        public HistoricoPromocao(Guid funcionarioId, string nivelAntigo, string novoNivel, string motivo)
        {
            Id = Guid.NewGuid();
            FuncionarioId = funcionarioId;
            DataPromocao = DateTime.UtcNow;
            NivelAntigo = nivelAntigo;
            NovoNivel = novoNivel;
            Motivo = motivo;
        }

        // Propriedade de navegação (opcional, se quiser ligar com Funcionario)
        // public virtual Funcionario? Funcionario { get; set; }
    }
}