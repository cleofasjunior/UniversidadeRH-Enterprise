using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using UniversidadeRH.Aplicacao.Interfaces;
using UniversidadeRH.Aplicacao.Servicos;
using UniversidadeRH.Dominio.Validadores;

namespace UniversidadeRH.Aplicacao;

public static class InjecaoDependencia
{
    public static IServiceCollection AddAplicacao(this IServiceCollection services)
    {
        // 1. Validadores Automáticos (FluentValidation)
        services.AddValidatorsFromAssemblyContaining<FuncionarioValidator>();

        // 2. Registrando os Serviços de Negócio
        services.AddScoped<IFuncionarioService, FuncionarioService>(); // Já existia
        
        // Novos Serviços
        services.AddScoped<ICarreiraService, CarreiraService>();
        services.AddScoped<IFeriasService, FeriasService>();
        services.AddScoped<ITreinamentoService, TreinamentoService>();
        services.AddScoped<IBeneficioService, BeneficioService>(); // Assumindo que criaremos este similar aos outros
                
        return services;
    }
}