using Microsoft.EntityFrameworkCore;
using UniversidadeRH.Dominio.Entidades;
using UniversidadeRH.Dominio.Interfaces;
using UniversidadeRH.Infraestrutura.Persistencia;

namespace UniversidadeRH.Infraestrutura.Repositorios
{
    public class FuncionarioRepositorio : IFuncionarioRepositorio
    {
        private readonly UniversidadeDbContext _context;

        public FuncionarioRepositorio(UniversidadeDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Funcionario funcionario)
        {
            await _context.Funcionarios.AddAsync(funcionario);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Funcionario funcionario)
        {
            // === CORREÇÃO 1: AVALIAÇÕES (Já existente) ===
            if (funcionario.Avaliacoes != null)
            {
                foreach (var avaliacao in funcionario.Avaliacoes)
                {
                    var entry = _context.Entry(avaliacao);
                    if (entry.State == EntityState.Detached || entry.State == EntityState.Modified)
                    {
                        var existeNoBanco = await _context.Avaliacoes
                            .AsNoTracking()
                            .AnyAsync(a => a.Id == avaliacao.Id);

                        if (!existeNoBanco)
                        {
                            entry.State = EntityState.Added; 
                        }
                    }
                }
            }

            // === ATIVIDADES ACADÊMICAS ===
            // O EF Core confunde IDs gerados no cliente com Updates. 
            // Verificamos se a atividade existe; se não, forçamos INSERT.
            if (funcionario.AtividadesAcademicas != null)
            {
                foreach (var atividade in funcionario.AtividadesAcademicas)
                {
                    var entry = _context.Entry(atividade);

                    if (entry.State == EntityState.Detached || entry.State == EntityState.Modified)
                    {
                        // Usamos Set<T>() para garantir acesso mesmo se não houver DbSet explícito
                        var existeAtividade = await _context.Set<AtividadeAcademica>()
                            .AsNoTracking()
                            .AnyAsync(a => a.Id == atividade.Id);

                        if (!existeAtividade)
                        {
                            entry.State = EntityState.Added; // Força o INSERT
                        }
                    }
                }
            }

            

            await _context.SaveChangesAsync();
        }

        // --- MÉTODOS DE LEITURA (Mantidos iguais) ---

        public async Task<Funcionario?> ObterPorIdAsync(Guid id)
        {
            return await _context.Funcionarios
                .Include(f => f.Treinamentos) 
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Funcionario?> ObterPorEmailAsync(string email)
        {
            return await _context.Funcionarios.AsNoTracking().FirstOrDefaultAsync(f => f.Email == email);
        }

        public async Task<List<Funcionario>> ListarTodosAsync()
        {
            return await _context.Funcionarios.AsNoTracking().ToListAsync();
        }

        public async Task<bool> ExisteAsync(string cpf)
        {
            return await _context.Funcionarios.AnyAsync(f => f.Cpf == cpf);
        }

        public async Task<Funcionario?> ObterPorIdComAtividadesAsync(Guid id)
        {
            return await _context.Funcionarios.Include(f => f.AtividadesAcademicas).FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Funcionario?> ObterPorIdComAvaliacoesAsync(Guid id)
        {
            return await _context.Funcionarios.Include(f => f.Avaliacoes).FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Funcionario?> ObterPorIdComTreinamentosAsync(Guid id)
        {
            return await _context.Funcionarios
                .Include(f => f.Treinamentos)
                    .ThenInclude(ft => ft.Treinamento)
                .Include(f => f.Avaliacoes)
                .FirstOrDefaultAsync(f => f.Id == id);
        }
    }
}