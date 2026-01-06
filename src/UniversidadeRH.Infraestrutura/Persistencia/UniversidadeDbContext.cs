using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UniversidadeRH.Dominio.Entidades;

namespace UniversidadeRH.Infraestrutura.Persistencia;

public class UniversidadeDbContext : IdentityDbContext<IdentityUser>
{
    public UniversidadeDbContext(DbContextOptions<UniversidadeDbContext> options)
        : base(options)
    {
    }

    public DbSet<Funcionario> Funcionarios { get; set; }
    public DbSet<Beneficio> Beneficios { get; set; }
    public DbSet<Ferias> Ferias { get; set; }
    public DbSet<Treinamento> Treinamentos { get; set; }
    public DbSet<AtividadeAcademica> AtividadesAcademicas { get; set; }
    public DbSet<HistoricoPromocao> HistoricoPromocoes { get; set; }
    public DbSet<AvaliacaoDesempenho> Avaliacoes { get; set; }
    public DbSet<AtestadoMedico> AtestadosMedicos { get; set; }
    
    // Tabela de Junção (N:N)
    public DbSet<FuncionarioTreinamento> FuncionarioTreinamentos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Configuração do Identity (Login)

        // === 🚨 A CORREÇÃO CRUCIAL (TABELA DE JUNÇÃO) ===
        // Isso resolve o erro "ConcurrencyException / 0 rows affected"
        
        // 1. Define que a CHAVE da tabela é a união dos dois IDs
        modelBuilder.Entity<FuncionarioTreinamento>()
            .HasKey(ft => new { ft.FuncionarioId, ft.TreinamentoId });

        // 2. Relacionamento com Funcionario
        modelBuilder.Entity<FuncionarioTreinamento>()
            .HasOne(ft => ft.Funcionario)
            .WithMany(f => f.Treinamentos)
            .HasForeignKey(ft => ft.FuncionarioId);

        // 3. Relacionamento com Treinamento
        modelBuilder.Entity<FuncionarioTreinamento>()
            .HasOne(ft => ft.Treinamento)
            .WithMany(t => t.Funcionarios)
            .HasForeignKey(ft => ft.TreinamentoId);

        // === FIM DA CORREÇÃO ===

        // === PRECISÃO DE VALORES ===
        modelBuilder.Entity<AvaliacaoDesempenho>()
            .Property(p => p.Nota)
            .HasPrecision(4, 2); 

        modelBuilder.Entity<Beneficio>()
            .Property(p => p.Valor)
            .HasPrecision(18, 2);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UniversidadeDbContext).Assembly);
    }
}