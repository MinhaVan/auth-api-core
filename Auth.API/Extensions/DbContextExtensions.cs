using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Auth.Data.Context;
using System;
using Auth.Service.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace Auth.API.Extensions;

[ExcludeFromCodeCoverage]
public static class DbContextExtensions
{
    public static IServiceCollection AddCustomDbContext(this IServiceCollection services, SecretManager secretManager)
    {
        var connectionString = secretManager.ConnectionStrings.DefaultConnection;
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("A string de conexão não foi fornecida.");
        }

        services.AddDbContext<APIContext>(options =>
        {
            options.UseNpgsql(connectionString, b => b.MigrationsAssembly("Auth.API"));
        });

        Console.WriteLine("Configuração de bancos de dados realizada com sucesso!");

        return services;
    }
}
