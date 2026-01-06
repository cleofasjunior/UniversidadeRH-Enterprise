using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversidadeRH.Dominio.Entidades;

namespace UniversidadeRH.Infraestrutura.Persistencia.Configuracoes;

public class FuncionarioConfiguration : IEntityTypeConfiguration<Funcionario>
{
    public void Configure(EntityTypeBuilder<Funcionario> builder)
    {
        // Define o nome da tabela no SQL
        builder.ToTable("Funcionarios");

        // Chave Primária
        builder.HasKey(f => f.Id);

        // Configuração de Propriedades
        builder.Property(f => f.Nome)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

        builder.Property(f => f.Email)
            .IsRequired()
            .HasMaxLength(150)
            .HasColumnType("varchar(150)");

        // Índice Único (Regra de Banco): Não permite dois funcionários com mesmo email
        builder.HasIndex(f => f.Email).IsUnique();

        builder.Property(f => f.Departamento)
            .IsRequired()
            .HasMaxLength(100);

        // Sênior Tip: Salvar Enum como Texto ("Professor") facilita leitura do banco
        // Se salvar como int (1), o DBA não sabe o que significa.
        builder.Property(f => f.Tipo)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(f => f.LinkLattes)
            .HasMaxLength(300);
            
        // Campos de controle
        builder.Property(f => f.DataAdmissao).IsRequired();
        builder.Property(f => f.Ativo).IsRequired();
    }
}