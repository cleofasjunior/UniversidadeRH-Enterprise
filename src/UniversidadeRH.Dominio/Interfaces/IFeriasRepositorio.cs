using UniversidadeRH.Dominio.Entidades;

namespace UniversidadeRH.Dominio.Interfaces
{
    public interface IFeriasRepositorio
    {
        Task AdicionarAsync(Ferias ferias);
        Task<List<Ferias>> ObterPorFuncionarioAsync(Guid funcionarioId);
    }
}