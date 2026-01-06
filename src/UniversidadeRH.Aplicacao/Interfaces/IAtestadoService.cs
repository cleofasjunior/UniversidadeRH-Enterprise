using UniversidadeRH.Aplicacao.DTOs;
using UniversidadeRH.Dominio.Entidades;

namespace UniversidadeRH.Aplicacao.Interfaces
{
    public interface IAtestadoService
    {
        Task<AtestadoMedico> RegistrarAtestadoAsync(RegistrarAtestadoDto dto);
        Task<List<AtestadoMedico>> ConsultarHistoricoAsync(Guid funcionarioId);
    }
}