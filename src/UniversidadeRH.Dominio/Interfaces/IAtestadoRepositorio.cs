using UniversidadeRH.Dominio.Entidades;

namespace UniversidadeRH.Dominio.Interfaces
{
    public interface IAtestadoRepositorio
    {
        Task AdicionarAsync(AtestadoMedico atestado);
        Task<List<AtestadoMedico>> ObterPorFuncionarioAsync(Guid funcionarioId);
    }
}