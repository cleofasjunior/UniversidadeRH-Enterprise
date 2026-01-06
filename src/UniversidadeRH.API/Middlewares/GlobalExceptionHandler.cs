using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UniversidadeRH.Dominio.Excecoes;

namespace UniversidadeRH.API.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // 1. Logamos o erro (para o desenvolvedor saber o que houve)
        _logger.LogError(exception, "Ocorreu um erro inesperado: {Message}", exception.Message);

        // 2. Definimos o comportamento baseado no tipo do erro
        var problemDetails = new ProblemDetails
        {
            Instance = httpContext.Request.Path
        };

        if (exception is DomainException)
        {
            // Erro de Regra de Negócio (O usuário fez algo errado ou regra violada)
            // Retornamos 400 Bad Request e MOSTRAMOS a mensagem segura.
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            problemDetails.Title = "Erro de Validação";
            problemDetails.Detail = exception.Message;
            problemDetails.Type = "https://universidaderh.com/erros/validacao";
        }
        else
        {
            // Erro Inesperado (Bug, Banco caiu, NullReference)
            // Retornamos 500 Internal Server Error e ESCONDEMOS a mensagem técnica.
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            problemDetails.Title = "Erro Interno do Servidor";
            problemDetails.Detail = "Ocorreu um erro interno. Contate o suporte."; // Mensagem genérica segura
            problemDetails.Type = "https://universidaderh.com/erros/interno";
        }

        // 3. Escrevemos a resposta JSON padronizada (RFC 7807)
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        // Retorna true indicando que o erro foi tratado e não precisa subir mais
        return true;
    }
}