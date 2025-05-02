using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.API.Extensions;

[ExcludeFromCodeCoverage]
public static class ResponseCompressionExtensions
{
    public static IServiceCollection AddCustomResponseCompression(this IServiceCollection services)
    {
        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<GzipCompressionProvider>();
            options.Providers.Add<BrotliCompressionProvider>();
        });

        Console.WriteLine("Configuração de compressão de resposta realizada com sucesso!");

        return services;
    }
}
