using UniversidadeRH.Aplicacao.DTOs;
using UniversidadeRH.Aplicacao.Interfaces;
using UniversidadeRH.Dominio.Entidades;
using UniversidadeRH.Dominio.Interfaces;

namespace UniversidadeRH.Aplicacao.Servicos
{
    public class AtestadoService : IAtestadoService
    {
        private readonly IAtestadoRepositorio _repositorio;
        // Opcional: Injetar IFuncionarioRepositorio para verificar se o funcionário existe

        public AtestadoService(IAtestadoRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<AtestadoMedico> RegistrarAtestadoAsync(RegistrarAtestadoDto dto)
        {
            var atestado = new AtestadoMedico(
                dto.FuncionarioId,
                dto.DataEmissao,
                dto.DiasAfastamento,
                dto.NomeMedico,
                dto.CRM,
                dto.CID
            );

            await _repositorio.AdicionarAsync(atestado);
            return atestado;
        }

        public async Task<List<AtestadoMedico>> ConsultarHistoricoAsync(Guid funcionarioId)
        {
            return await _repositorio.ObterPorFuncionarioAsync(funcionarioId);
        }
    }
}