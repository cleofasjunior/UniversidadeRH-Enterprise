using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniversidadeRH.Dominio.Interfaces;
using UniversidadeRH.Infraestrutura.Persistencia;
using UniversidadeRH.Infraestrutura.Repositorios;

namespace UniversidadeRH.Infraestrutura;

public static class InjecaoDependencia
{
    public static IServiceCollection AddInfraestrutura(this IServiceCollection services, IConfiguration configuration)
    {
       // 1. Configuração do Banco de Dados
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<UniversidadeDbContext>(options =>
            options.UseSqlServer(connectionString, 
                b => 
                {
                    b.MigrationsAssembly(typeof(UniversidadeDbContext).Assembly.FullName);
                    b.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null); // <--- LINHA MÁGICA
                }));

        // 2. Configuração do Identity (Login e Senha)
        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<UniversidadeDbContext>()
            .AddDefaultTokenProviders();

        // 3. Registro dos Repositórios 
        // O sistema precisa saber qual classe usar para cada interface
        
        services.AddScoped<IFuncionarioRepositorio, FuncionarioRepositorio>();
        
        // Adicionamos os novos repositórios que faltavam:
        services.AddScoped<IBeneficioRepositorio, BeneficioRepositorio>();
        services.AddScoped<ITreinamentoRepositorio, TreinamentoRepositorio>();

        return services;
    }
}