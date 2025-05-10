using System;
using System.Diagnostics.CodeAnalysis;
using Auth.Data.Implementations;
using Auth.Data.Repositories;
using Auth.Domain.Interfaces.Repositories;
using Auth.Domain.Interfaces.Repository;
using Auth.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.API.Extensions;

[ExcludeFromCodeCoverage]
public static class RepositoryExtensions
{
    public static IServiceCollection AddCustomRepository(
        this IServiceCollection services)
    {
        services.AddScoped<IUserContext, UserContext>();

        // Repositories
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IBaseRepository<Endereco>, BaseRepository<Endereco>>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IBaseRepository<Empresa>, BaseRepository<Empresa>>();
        services.AddScoped<IBaseRepository<UsuarioPermissao>, BaseRepository<UsuarioPermissao>>();
        services.AddScoped<IBaseRepository<Permissao>, BaseRepository<Permissao>>();
        services.AddScoped<IBaseRepository<Aluno>, BaseRepository<Aluno>>();
        services.AddScoped<IRedisRepository, RedisRepository>();
        services.AddScoped<IPermissaoRepository, PermissaoRepository>();

        Console.WriteLine("Configuração de repository realizada com sucesso!");

        return services;
    }
}