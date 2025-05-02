using System;
using System.Diagnostics.CodeAnalysis;
using Auth.Service.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.API.Extensions;

[ExcludeFromCodeCoverage]
public static class HttpExtensions
{
    public static IServiceCollection AddCustomHttp(this IServiceCollection services, SecretManager secretManager)
    {
        services.AddHttpClient("api-nominatim", client =>
        {
            client.BaseAddress = new Uri("https://nominatim.openstreetmap.org/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        services.AddHttpClient("api-googlemaps", client =>
        {
            client.BaseAddress = new Uri(secretManager.Google.BaseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        Console.WriteLine("Configuração das APIs consumidas realizada com sucesso!");

        return services;
    }
}
