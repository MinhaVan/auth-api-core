using Auth.Data.Implementations;
using Auth.Domain.Interfaces.Repositories;
using Auth.Service.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Auth.API.Extensions;

public static class QueueExtensions
{
    public static IServiceCollection AddQueue(this IServiceCollection services, SecretManager secretManager)
    {
        services.AddSingleton<IConnectionFactory>(sp =>
        {
            return new ConnectionFactory
            {
                UserName = secretManager.Infra.RabbitMQ.UserName,
                Password = secretManager.Infra.RabbitMQ.Password,
                HostName = secretManager.Infra.RabbitMQ.Host,
                Port = int.Parse(secretManager.Infra.RabbitMQ.Port)
            };
        });

        services.AddScoped<IRabbitMqRepository, RabbitMqRepository>();
        return services;
    }
}