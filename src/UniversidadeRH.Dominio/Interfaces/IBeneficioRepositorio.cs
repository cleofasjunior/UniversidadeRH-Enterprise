using UniversidadeRH.Dominio.Entidades;

namespace UniversidadeRH.Dominio.Interfaces
{
    public interface IBeneficioRepositorio
    {
        Task AdicionarAsync(Beneficio beneficio);
        Task<Beneficio?> ObterPorIdAsync(Guid id);
        Task<List<Beneficio>> ListarTodosAsync();
        Task VincularBeneficioAsync(Guid funcionarioId, Guid beneficioId);
    }
}