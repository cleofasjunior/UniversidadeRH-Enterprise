using UniversidadeRH.Dominio.Excecoes; 

namespace UniversidadeRH.Dominio.Entidades
{
    public class AtestadoMedico
    {
        public Guid Id { get; private set; }
        public Guid FuncionarioId { get; private set; }
        public DateTime DataEmissao { get; private set; }
        public int DiasAfastamento { get; private set; }
        public string NomeMedico { get; private set; }
        public string CRM { get; private set; }
        public string CID { get; private set; } // Código da Doença (Opcional)

        // 👇 Inicializamos as strings com null! 
        protected AtestadoMedico() 
        { 
            NomeMedico = null!;
            CRM = null!;
            CID = null!;
        }

        public AtestadoMedico(Guid funcionarioId, DateTime dataEmissao, int dias, string nomeMedico, string crm, string cid)
        {
            if (dias <= 0) throw new DomainException("Atestado deve ter no mínimo 1 dia de afastamento.");
            if (string.IsNullOrEmpty(crm)) throw new DomainException("O CRM do médico é obrigatório.");

            Id = Guid.NewGuid();
            FuncionarioId = funcionarioId;
            DataEmissao = dataEmissao;
            DiasAfastamento = dias;
            NomeMedico = nomeMedico;
            CRM = crm;
            
            // Se o CID for opcional e vier vazio, garantimos que não fica nulo
            CID = cid ?? string.Empty; 
        }
    }
}