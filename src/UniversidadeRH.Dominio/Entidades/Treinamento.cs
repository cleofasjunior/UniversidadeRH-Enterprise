using UniversidadeRH.Dominio.Enums;

namespace UniversidadeRH.Dominio.Entidades
{
    public class Treinamento
    {
        public Guid Id { get; private set; }
        public string Descricao { get; private set; }
        public NivelTreinamento Nivel { get; private set; }
        public TipoFuncionario PublicoAlvo { get; private set; }
        public int CargaHoraria { get; private set; }

        // --- PROPRIEDADE DE NAVEGAÇÃO ---
        // O EF precisa disto para o comando .WithMany(t => t.Funcionarios) funcionar.
        // Representa a tabela de ligação vista pelo lado do Treinamento.
        public virtual ICollection<FuncionarioTreinamento> Funcionarios { get; private set; } = new List<FuncionarioTreinamento>();

        protected Treinamento() 
        { 
            Descricao = null!; 
        }

        public Treinamento(string descricao, NivelTreinamento nivel, TipoFuncionario publicoAlvo, int cargaHoraria)
        {
            Id = Guid.NewGuid();
            Descricao = descricao;
            Nivel = nivel;
            PublicoAlvo = publicoAlvo;
            CargaHoraria = cargaHoraria;
        }
    }
}