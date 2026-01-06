using UniversidadeRH.Aplicacao.DTOs; 
using UniversidadeRH.Aplicacao.Interfaces;
using UniversidadeRH.Dominio.Entidades;
using UniversidadeRH.Dominio.Interfaces;

namespace UniversidadeRH.Aplicacao.Servicos
{
    public class BeneficioService : IBeneficioService
    {
        private readonly IBeneficioRepositorio _repositorio;

        public BeneficioService(IBeneficioRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        
        public async Task<Beneficio> CriarBeneficioAsync(CriarBeneficioDto dto)
        {
            var beneficio = new Beneficio(dto.Nome, dto.Valor);
            await _repositorio.AdicionarAsync(beneficio);
            return beneficio;
        }

        // 👇 CORREÇÃO: Agora recebe o DTO inteiro
        public async Task VincularBeneficioAsync(VincularBeneficioDto dto)
        {
            await _repositorio.VincularBeneficioAsync(dto.FuncionarioId, dto.BeneficioId);
        }
        
        public async Task<List<Beneficio>> ListarDisponiveisAsync()
        {
            return await _repositorio.ListarTodosAsync();
        }
    }
}