using Microsoft.EntityFrameworkCore;
using UniversidadeRH.Dominio.Entidades;
using UniversidadeRH.Dominio.Interfaces;
using UniversidadeRH.Infraestrutura.Persistencia;

namespace UniversidadeRH.Infraestrutura.Repositorios
{
    public class BeneficioRepositorio : IBeneficioRepositorio
    {
        private readonly UniversidadeDbContext _context;

        public BeneficioRepositorio(UniversidadeDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Beneficio beneficio)
        {
            await _context.Beneficios.AddAsync(beneficio);
            await _context.SaveChangesAsync();
        }

        public async Task<Beneficio?> ObterPorIdAsync(Guid id)
        {
            return await _context.Beneficios.FindAsync(id);
        }

        public async Task<List<Beneficio>> ListarTodosAsync()
        {
            return await _context.Beneficios.AsNoTracking().ToListAsync();
        }

        public async Task VincularBeneficioAsync(Guid funcionarioId, Guid beneficioId)
        {
            // Busca as entidades
            var funcionario = await _context.Funcionarios.FindAsync(funcionarioId);
            var beneficio = await _context.Beneficios.FindAsync(beneficioId);

            if (funcionario != null && beneficio != null)
            {
                // Adiciona o relacionamento (EF Core entende a tabela de junção BeneficioFuncionario)
                funcionario.Beneficios.Add(beneficio);
                await _context.SaveChangesAsync();
            }
        }
    }
}