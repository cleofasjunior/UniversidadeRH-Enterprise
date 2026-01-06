using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using UniversidadeRH.Infraestrutura.Persistencia; 
using UniversidadeRH.Dominio.Interfaces;
using UniversidadeRH.Infraestrutura.Repositorios;
using UniversidadeRH.Aplicacao.Interfaces;
using UniversidadeRH.Aplicacao.Servicos;
using FluentValidation; 
using UniversidadeRH.Dominio.Validadores;
using UniversidadeRH.Aplicacao.Validadores; 
using UniversidadeRH.API.Middlewares;
using Serilog;

// Configuração Inicial do Serilog (Bootstrap)
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Ativa o Serilog no Host
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day));

    // ==================================================================
    // 1. CONFIGURAÇÃO DO BANCO DE DADOS
    // ==================================================================
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<UniversidadeDbContext>(options =>
        options.UseSqlServer(connectionString));

    // ==================================================================
    // 2. CONFIGURAÇÃO DE IDENTITY
    // ==================================================================
    builder.Services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<UniversidadeDbContext>()
        .AddDefaultTokenProviders();

    // ==================================================================
    // 3. CONFIGURAÇÃO DE JWT
    // ==================================================================
    var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"] ?? "MinhaChaveSuperSecretaDeDesenvolvimento2026!");

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false, 
            ValidateAudience = false 
        };
    });

    // ==================================================================
    // 4. INJEÇÃO DE DEPENDÊNCIA
    // ==================================================================
    
    // Módulo Funcionários
    builder.Services.AddScoped<IFuncionarioRepositorio, FuncionarioRepositorio>();
    builder.Services.AddScoped<IFuncionarioService, FuncionarioService>();
    
    // --- CORREÇÃO DO ERRO DO FLUENT VALIDATION AQUI ---
    
    // 1. Validação do Domínio (Entidades)
    builder.Services.AddValidatorsFromAssemblyContaining<FuncionarioValidator>();

    // 2. Validação da Aplicação (DTOs)
    // A linha abaixo registra o RegistrarAvaliacaoValidator e resolve o erro 500
    builder.Services.AddValidatorsFromAssemblyContaining<RegistrarAvaliacaoValidator>();

    // Outros Módulos
    builder.Services.AddScoped<IBeneficioRepositorio, BeneficioRepositorio>();
    builder.Services.AddScoped<IBeneficioService, BeneficioService>();
    builder.Services.AddScoped<IFeriasRepositorio, FeriasRepositorio>();
    builder.Services.AddScoped<IFeriasService, FeriasService>();
    builder.Services.AddScoped<ICarreiraRepositorio, CarreiraRepositorio>();
    builder.Services.AddScoped<ICarreiraService, CarreiraService>();
    builder.Services.AddScoped<IAtestadoRepositorio, AtestadoRepositorio>();
    builder.Services.AddScoped<IAtestadoService, AtestadoService>();

    // Módulo Treinamento
    builder.Services.AddScoped<ITreinamentoRepositorio, TreinamentoRepositorio>();
    builder.Services.AddScoped<ITreinamentoService, TreinamentoService>();

    // ==================================================================
    // 5. SERVIÇOS BÁSICOS & TRATAMENTO DE ERROS
    // ==================================================================
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    
    builder.Services.AddProblemDetails();
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

    // ==================================================================
    // 6. CONFIGURAÇÃO DO SWAGGER
    // ==================================================================
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "UniversidadeRH Enterprise API", Version = "v1" });
        
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Name = "Authorization", Type = SecuritySchemeType.ApiKey, Scheme = "Bearer", BearerFormat = "JWT", In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme."
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
                new string[] {}
            }
        });
    });

    var app = builder.Build();

    // ==================================================================
    // 7. PIPELINE DE EXECUÇÃO
    // ==================================================================

    app.UseExceptionHandler(); // Middleware de Erro Global

    app.UseSerilogRequestLogging(); // Log de Requisições HTTP

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthentication(); 
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "A aplicação falhou ao iniciar!");
}
finally
{
    Log.CloseAndFlush();
}