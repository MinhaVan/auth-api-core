using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.API.Extensions;

[ExcludeFromCodeCoverage]
public static class CorsExtensions
{
    public static IServiceCollection AddCustomCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed(_ => true) // Permitir todas as origens
                    .AllowCredentials();
            });
        });

        Console.WriteLine("Configuração do CORS realizada com sucesso!");

        return services;
    }
}
