using Microsoft.EntityFrameworkCore;
using UniversidadeRH.Dominio.Entidades;
using UniversidadeRH.Dominio.Interfaces;
using UniversidadeRH.Infraestrutura.Persistencia; 

namespace UniversidadeRH.Infraestrutura.Repositorios
{
    public class FeriasRepositorio : IFeriasRepositorio
    {
        private readonly UniversidadeDbContext _context;

        public FeriasRepositorio(UniversidadeDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Ferias ferias)
        {
            await _context.Ferias.AddAsync(ferias);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Ferias>> ObterPorFuncionarioAsync(Guid funcionarioId)
        {
            return await _context.Ferias
                .AsNoTracking()
                .Where(f => f.FuncionarioId == funcionarioId)
                .ToListAsync();
        }
    }
}