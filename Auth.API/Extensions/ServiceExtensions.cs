using System;
using Auth.Service.Implementations;
using Auth.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Auth.API.Filters;
using Auth.API.Converters;
using FluentValidation.AspNetCore;
using FluentValidation;
using System.Reflection;
using StackExchange.Redis;
using Auth.Service.Configuration;
// using Auth.Service.Workers;

namespace Auth.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services, SecretManager secretManager)
    {
        services.AddHttpContextAccessor();
        services.AddCache(secretManager);

        services.AddScoped<IAmazonService, AmazonService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEnderecoService, EnderecoService>();
        services.AddScoped<IUsuarioService, UsuarioService>();

        Console.WriteLine("Configuração das services realizada com sucesso!");

        return services;
    }

    public static IServiceCollection AddCache(this IServiceCollection services, SecretManager secretManager)
    {
        services.AddSignalR().AddStackExchangeRedis(secretManager.ConnectionStrings.RedisConnection, options =>
        {
            options.Configuration.ChannelPrefix = "rotaHub"; // Nome opcional
        });

        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var configuration = secretManager.ConnectionStrings.RedisConnection;
            return ConnectionMultiplexer.Connect(configuration);
        });

        Console.WriteLine("Configuração do Redis realizada com sucesso!");

        return services;
    }

    public static IServiceCollection AddCustomMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());
        return services;
    }

    public static IServiceCollection AddControllersWithFilters(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<GlobalExceptionFilter>();
        }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
        });

        services.AddSignalR();

        services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}