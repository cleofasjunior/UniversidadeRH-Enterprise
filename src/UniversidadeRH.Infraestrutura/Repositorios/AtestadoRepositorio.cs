using Microsoft.EntityFrameworkCore;
using UniversidadeRH.Dominio.Entidades;
using UniversidadeRH.Dominio.Interfaces;
using UniversidadeRH.Infraestrutura.Persistencia;

namespace UniversidadeRH.Infraestrutura.Repositorios
{
    public class AtestadoRepositorio : IAtestadoRepositorio
    {
        private readonly UniversidadeDbContext _context;

        public AtestadoRepositorio(UniversidadeDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(AtestadoMedico atestado)
        {
            await _context.AtestadosMedicos.AddAsync(atestado);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AtestadoMedico>> ObterPorFuncionarioAsync(Guid funcionarioId)
        {
            return await _context.AtestadosMedicos
                .AsNoTracking()
                .Where(a => a.FuncionarioId == funcionarioId)
                .OrderByDescending(a => a.DataEmissao)
                .ToListAsync();
        }
    }
}