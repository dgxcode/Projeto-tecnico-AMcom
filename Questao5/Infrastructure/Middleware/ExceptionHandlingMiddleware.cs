
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Questao5.Domain.Language;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Questao5.Infrastructure.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BusinessException businessEx)
            {
                _logger.LogWarning(businessEx, "Erro de negócio.");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var result = JsonSerializer.Serialize(new
                {
                    erro = businessEx.Message,
                    tipo = businessEx.Tipo
                });

                await context.Response.WriteAsync(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro não tratado.");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var result = JsonSerializer.Serialize(new { erro = ex.Message });
                await context.Response.WriteAsync(result);
            }
        }
    }
}

