using UniversidadeRH.Dominio.Enums;
using UniversidadeRH.Dominio.Excecoes;

namespace UniversidadeRH.Dominio.Entidades
{
    public class Funcionario
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Cpf { get; private set; } 
        public string Departamento { get; private set; }
        public TipoFuncionario Tipo { get; private set; }
        public string? LinkLattes { get; private set; }
        public DateTime DataAdmissao { get; private set; }
        public bool Ativo { get; private set; }

        // --- PROPRIEDADES DE CARREIRA (DOCENTE) ---
        public RegimeTrabalho? Regime { get; private set; }
        public NivelCarreira? NivelCarreira { get; private set; }
        public int PontosCarreira { get; private set; }
        public virtual ICollection<AtividadeAcademica> AtividadesAcademicas { get; private set; } = new List<AtividadeAcademica>();

        // --- PROPRIEDADES DE CARREIRA (TÉCNICO) ---
        public NivelTecnico? NivelTecnico { get; private set; }
        public DateTime? DataUltimaPromocao { get; private set; }

        // --- LISTAS DE RELACIONAMENTO ---
        public virtual ICollection<Beneficio> Beneficios { get; private set; } = new List<Beneficio>();
        public virtual ICollection<Ferias> HistoricoFerias { get; private set; } = new List<Ferias>();
        public virtual ICollection<AvaliacaoDesempenho> Avaliacoes { get; private set; } = new List<AvaliacaoDesempenho>();
        public virtual ICollection<FuncionarioTreinamento> Treinamentos { get; private set; } = new List<FuncionarioTreinamento>();

        // Construtor vazio para o Entity Framework
        protected Funcionario() 
        { 
            Nome = null!;
            Email = null!;
            Cpf = null!; 
            Departamento = null!;
        }

        // Construtor principal utilizado pelo Service
        public Funcionario(string nome, string email, string cpf, string departamento, TipoFuncionario tipo, string? linkLattes)
        {
            Id = Guid.NewGuid();
            Nome = nome; 
            Email = email; 
            Cpf = cpf; 
            Departamento = departamento; 
            Tipo = tipo; 
            LinkLattes = linkLattes;
            DataAdmissao = DateTime.UtcNow; 
            Ativo = true;
            
            if (tipo == TipoFuncionario.Professor) NivelCarreira = Enums.NivelCarreira.Auxiliar;
            if (tipo == TipoFuncionario.TecnicoAdministrativo) NivelTecnico = Enums.NivelTecnico.Nivel_I;
        }

        // =================================================================
        // MÉTODOS DE NEGÓCIO
        // =================================================================

        // === MÉTODOS DE CONFIGURAÇÃO (Chamados pelo Service) ===

        public void DefinirRegime(RegimeTrabalho regime)
        {
            Regime = regime;
        }

        public void AlterarDataAdmissao(DateTime data)
        {
            DataAdmissao = data;
        }

        // =====================================================

        public void AdicionarBeneficio(Beneficio beneficio)
        {
            if (!Beneficios.Any(b => b.Id == beneficio.Id))
                Beneficios.Add(beneficio);
        }

        public void SolicitarFerias(DateTime inicio, int dias)
        {
            if (DataAdmissao.AddYears(1) > DateTime.UtcNow)
                throw new DomainException("Funcionário ainda não completou 1 ano de casa (Período Aquisitivo).");

            HistoricoFerias.Add(new Ferias(this.Id, inicio, dias));
        }

        public void AdicionarAvaliacao(AvaliacaoDesempenho avaliacao)
        {
            Avaliacoes.Add(avaliacao);
        }

        public void AdicionarTreinamento(Treinamento treinamento, string motivo)
        {
            Treinamentos.Add(new FuncionarioTreinamento(this.Id, treinamento.Id, motivo));
        }

       public void AdicionarAtividadeAcademica(AtividadeAcademica atividade)
        {
            if (Tipo != TipoFuncionario.Professor)
                throw new DomainException("Apenas professores possuem Atividades Acadêmicas.");

            if (Regime == null) throw new DomainException("Professor sem Regime de Trabalho definido.");

            var totalHoras = AtividadesAcademicas.Sum(a => a.HorasSemanais) + atividade.HorasSemanais;

            
            int limite = (int)Regime switch
            {
                1 => 20, // 20 Horas
                2 => 40, // 40 Horas
                3 => 40, // Dedicação Exclusiva (também é 40h de teto para atividades)
                _ => 20  // Padrão de segurança
            };
            // =====================

            if (totalHoras > limite)
                throw new DomainException($"A atividade excede o limite de horas do regime ({limite}h).");

            AtividadesAcademicas.Add(atividade);
        }
        public void AdicionarPontosCarreira(int pontos, string motivo)
        {
            PontosCarreira += pontos;
            AvaliarPromocaoAcademica();
        }

        private void AvaliarPromocaoAcademica()
        {
            if (NivelCarreira == Enums.NivelCarreira.Auxiliar && PontosCarreira >= 100)
                NivelCarreira = Enums.NivelCarreira.Assistente;
            else if (NivelCarreira == Enums.NivelCarreira.Assistente && PontosCarreira >= 300)
                NivelCarreira = Enums.NivelCarreira.Adjunto;
            else if (NivelCarreira == Enums.NivelCarreira.Adjunto && PontosCarreira >= 1000)
                NivelCarreira = Enums.NivelCarreira.Titular;
        }

        public void TentarPromocao()
        {
            if (Tipo == TipoFuncionario.TecnicoAdministrativo)
            {
                var dataBase = DataUltimaPromocao ?? DataAdmissao;
                if ((DateTime.UtcNow - dataBase).TotalDays >= 730) 
                {
                    if (Avaliacoes.Any() && Avaliacoes.Average(a => a.Nota) >= 7.0m)
                    {
                       SubirNivelTecnico();
                    }
                }
            }
            else if (Tipo == TipoFuncionario.Professor)
            {
                AvaliarPromocaoAcademica();
            }
        }

        private void SubirNivelTecnico()
        {
            if (NivelTecnico.HasValue && NivelTecnico.Value < UniversidadeRH.Dominio.Enums.NivelTecnico.Nivel_IV)
            {
                NivelTecnico = NivelTecnico.Value + 1;
                DataUltimaPromocao = DateTime.UtcNow;
            }
            else if (!NivelTecnico.HasValue)
            {
                NivelTecnico = Enums.NivelTecnico.Nivel_I;
                DataUltimaPromocao = DateTime.UtcNow;
            }
        }
    }
}