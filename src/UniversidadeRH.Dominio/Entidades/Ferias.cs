using UniversidadeRH.Dominio.Excecoes;

namespace UniversidadeRH.Dominio.Entidades;

public class Ferias
{
    public Guid Id { get; private set; }
    public Guid FuncionarioId { get; private set; }
    public DateTime DataInicio { get; private set; }
    public DateTime DataFim { get; private set; }
    public bool Aprovado { get; private set; }

    public Funcionario Funcionario { get; private set; } = null!;

    protected Ferias() { }

    public Ferias(Guid funcionarioId, DateTime inicio, int dias)
    {
        if (dias < 5 || dias > 30) throw new DomainException("Férias devem ser entre 5 e 30 dias.");
        if (inicio < DateTime.UtcNow.Date) throw new DomainException("Férias não podem ser retroativas.");
        
        Id = Guid.NewGuid();
        FuncionarioId = funcionarioId;
        DataInicio = inicio;
        DataFim = inicio.AddDays(dias);
        Aprovado = false;
    }
}