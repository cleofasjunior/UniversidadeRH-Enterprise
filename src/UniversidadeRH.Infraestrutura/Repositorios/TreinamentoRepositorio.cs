using Microsoft.EntityFrameworkCore;
using UniversidadeRH.Dominio.Entidades;
using UniversidadeRH.Dominio.Interfaces;
using UniversidadeRH.Dominio.Enums; 
using UniversidadeRH.Infraestrutura.Persistencia;

namespace UniversidadeRH.Infraestrutura.Repositorios
{
    public class TreinamentoRepositorio : ITreinamentoRepositorio
    {
        private readonly UniversidadeDbContext _context;

        public TreinamentoRepositorio(UniversidadeDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Treinamento treinamento)
        {
            await _context.Treinamentos.AddAsync(treinamento);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Treinamento>> ListarTodosAsync()
        {
            return await _context.Treinamentos.AsNoTracking().ToListAsync();
        }

        public async Task<List<FuncionarioTreinamento>> ObterPendentesPorFuncionarioAsync(Guid funcionarioId)
        {
            return await _context.FuncionarioTreinamentos
                .Include(ft => ft.Treinamento)
                .Where(ft => ft.FuncionarioId == funcionarioId && !ft.Concluido)
                .ToListAsync();
        }

        public async Task<List<Treinamento>> ListarPorNivelETipoAsync(int nivel, TipoFuncionario tipo)
        {
            // Converte o int recebido para o Enum NivelTreinamento na consulta
            return await _context.Treinamentos
                .AsNoTracking()
                .Where(t => (int)t.Nivel == nivel && t.PublicoAlvo == tipo)
                .ToListAsync();
        }
    }
}