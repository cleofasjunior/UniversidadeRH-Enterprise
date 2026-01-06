namespace UniversidadeRH.Dominio.Excecoes;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}