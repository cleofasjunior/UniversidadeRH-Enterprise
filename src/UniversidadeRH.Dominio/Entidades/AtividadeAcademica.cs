using UniversidadeRH.Dominio.Enums;
using UniversidadeRH.Dominio.Excecoes;

namespace UniversidadeRH.Dominio.Entidades;

public class AtividadeAcademica
{
    public Guid Id { get; private set; }
    public Guid FuncionarioId { get; private set; }
    public string Descricao { get; private set; }
    public TipoAtividade Tipo { get; private set; }
    public int HorasSemanais { get; private set; }
    
    // Propriedade de navegação para o EF Core
    public Funcionario Funcionario { get; private set; } = null!;

   protected AtividadeAcademica() 
    { 
        Descricao = null!; 
    }

    public AtividadeAcademica(Guid funcionarioId, string descricao, TipoAtividade tipo, int horas)
    {
        if (horas <= 0) throw new DomainException("Horas devem ser maior que zero.");
        Id = Guid.NewGuid();
        FuncionarioId = funcionarioId;
        Descricao = descricao;
        Tipo = tipo;
        HorasSemanais = horas;
    }
}