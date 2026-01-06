using UniversidadeRH.Aplicacao.DTOs;
using UniversidadeRH.Dominio.Entidades;

namespace UniversidadeRH.Aplicacao.Interfaces
{
    public interface IFeriasService
    {
        Task<Ferias> SolicitarFeriasAsync(Guid funcionarioId, DateTime inicio, DateTime fim);
        Task<List<Ferias>> ConsultarFeriasDoFuncionarioAsync(Guid funcionarioId);
    }
}