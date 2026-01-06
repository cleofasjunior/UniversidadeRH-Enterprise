using UniversidadeRH.Aplicacao.DTOs;
using UniversidadeRH.Dominio.Entidades;

namespace UniversidadeRH.Aplicacao.Interfaces
{
    public interface IBeneficioService
    {
       
        Task<Beneficio> CriarBeneficioAsync(CriarBeneficioDto dto);

        Task VincularBeneficioAsync(VincularBeneficioDto dto);
        
        Task<List<Beneficio>> ListarDisponiveisAsync();
    }
}