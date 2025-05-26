using System;
using System.Diagnostics.CodeAnalysis;
using Auth.Data.APIs;
using Auth.Domain.Interfaces.APIs;
using Auth.Service.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Routes.Data.APIs;

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

        services.AddHttpClient("api-routes", client =>
        {
            client.BaseAddress = new Uri(secretManager.URL.RoutesAPI);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        services.AddHttpClient("api-pessoas", client =>
        {
            client.BaseAddress = new Uri(secretManager.URL.PessoasAPI);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        services.AddScoped<IRoutesAPI, RoutesAPI>();
        services.AddScoped<IPessoasAPI, PessoasAPI>();


        Console.WriteLine("Configuração das APIs consumidas realizada com sucesso!");

        return services;
    }
}
