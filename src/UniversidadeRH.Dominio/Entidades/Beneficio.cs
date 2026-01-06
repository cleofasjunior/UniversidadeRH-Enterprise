namespace UniversidadeRH.Dominio.Entidades; // <--- O Namespace TEM que ser esse

public class Beneficio
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public decimal Valor { get; private set; }
    public bool Ativo { get; private set; }
    
    // Relação EF Core
    public ICollection<Funcionario> Funcionarios { get; private set; } = new List<Funcionario>();

   protected Beneficio() { 
    Nome = null!; 
}
    
    public Beneficio(string nome, decimal valor) 
    {
        Id = Guid.NewGuid(); 
        Nome = nome; 
        Valor = valor; 
        Ativo = true;
    }
}