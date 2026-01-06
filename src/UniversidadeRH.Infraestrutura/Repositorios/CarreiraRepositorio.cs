using Microsoft.EntityFrameworkCore;
using UniversidadeRH.Dominio.Entidades;
using UniversidadeRH.Dominio.Interfaces;
using UniversidadeRH.Infraestrutura.Persistencia; 

namespace UniversidadeRH.Infraestrutura.Repositorios
{
    public class CarreiraRepositorio : ICarreiraRepositorio
    {
        private readonly UniversidadeDbContext _context;

        public CarreiraRepositorio(UniversidadeDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAtividadeAsync(AtividadeAcademica atividade)
        {
            await _context.AtividadesAcademicas.AddAsync(atividade);
            await _context.SaveChangesAsync();
        }

        public async Task RegistrarPromocaoAsync(HistoricoPromocao promocao)
        {
            await _context.HistoricoPromocoes.AddAsync(promocao);
            await _context.SaveChangesAsync();
        }

        public async Task<Funcionario?> ObterFuncionarioAsync(Guid id)
        {
            return await _context.Funcionarios.FindAsync(id);
        }
    }
}