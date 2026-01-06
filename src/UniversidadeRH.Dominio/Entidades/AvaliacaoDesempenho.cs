using UniversidadeRH.Dominio.Excecoes;

namespace UniversidadeRH.Dominio.Entidades
{
    public class AvaliacaoDesempenho
    {
        public Guid Id { get; private set; }
        public Guid FuncionarioId { get; private set; }
        public decimal Nota { get; private set; }
        public string Feedback { get; private set; }
        public DateTime DataAvaliacao { get; private set; }

        // Inicializado inline para evitar Warning no Funcionario também
        public virtual Funcionario Funcionario { get; private set; } = null!;

        // === CORREÇÃO DO ERRO CS8618 AQUI ===
        protected AvaliacaoDesempenho() 
        { 
        
            // O EF Core vai preencher com o valor real do banco de dados.
            Feedback = null!; 
        }

        public AvaliacaoDesempenho(Guid funcionarioId, decimal nota, string feedback)
        {
            if (nota < 0 || nota > 10) throw new DomainException("Nota deve ser entre 0 e 10.");
            
            Id = Guid.NewGuid(); // ✅ Gera ID novo corretamente
            FuncionarioId = funcionarioId;
            Nota = nota;
            Feedback = feedback;
            DataAvaliacao = DateTime.UtcNow;
        }
    }
}