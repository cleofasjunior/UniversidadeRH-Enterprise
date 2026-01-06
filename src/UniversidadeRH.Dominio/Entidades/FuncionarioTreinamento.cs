using UniversidadeRH.Dominio.Excecoes;

namespace UniversidadeRH.Dominio.Entidades
{
    public class FuncionarioTreinamento
    {
        // Se a chave primária no banco for Composta (FuncionarioId + TreinamentoId), 
        // este Id aqui será apenas uma coluna extra, o que não tem problema.
        public Guid Id { get; private set; } 
        
        public Guid FuncionarioId { get; private set; }
        public Guid TreinamentoId { get; private set; }
        public string MotivoIndicacao { get; private set; }
        public DateTime DataIndicacao { get; private set; }
        public bool Concluido { get; private set; }

        // --- PROPRIEDADES DE NAVEGAÇÃO  ---
        // Adicionamos 'Funcionario'. Sem ela, o comando .HasOne(ft => ft.Funcionario) falha.
        public virtual Funcionario Funcionario { get; private set; } = null!;
        public virtual Treinamento Treinamento { get; private set; } = null!;

        protected FuncionarioTreinamento() 
        { 
            MotivoIndicacao = null!;
        }

        public FuncionarioTreinamento(Guid funcionarioId, Guid treinamentoId, string motivo)
        {
            Id = Guid.NewGuid();
            FuncionarioId = funcionarioId;
            TreinamentoId = treinamentoId;
            MotivoIndicacao = motivo;
            DataIndicacao = DateTime.UtcNow;
            Concluido = false;
        }

        public void Concluir()
        {
            Concluido = true;
        }
    }
}