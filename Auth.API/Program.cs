using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using AspNetCoreRateLimit;
using Auth.API.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Prometheus;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using Auth.Data.Context;
using Microsoft.EntityFrameworkCore;


namespace Auth.API;

[ExcludeFromCodeCoverage]
public static class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var environment = builder.Environment.EnvironmentName;
        Console.WriteLine($"Iniciando a API no ambiente '{environment}'");

        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        // Adiciona as configurações do Secrets Manager
        var secretManager = builder.Services.AddSecretManager(builder.Configuration);
        Console.WriteLine($"Iniciando a API no ambiente '{JsonConvert.SerializeObject(secretManager)}'");

        // Configura os serviços
        builder.Services.AddCustomAuthentication(secretManager)
                        .AddCustomAuthorization()
                        .AddCustomDbContext(secretManager)
                        .AddCustomSwagger()
                        .AddCustomRateLimiting(secretManager)
                        .AddCustomResponseCompression()
                        .AddCustomCors()
                        .AddCustomServices(secretManager)
                        .AddCustomRepository()
                        .AddCustomMapper()
                        .AddControllersWithFilters()
                        .AddCustomHttp(secretManager);

        // Configura o logger
        builder.Logging.ClearProviders().AddConsole().AddDebug();

        var app = builder.Build();

        // Configurações específicas para desenvolvimento
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth.API v1"));
        }
        else
        {
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<APIContext>();
                db.Database.Migrate();
            }

            app.UsePathBase("/auth");
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/auth/swagger/v1/swagger.json", "Auth.API v1");
                c.RoutePrefix = "swagger";
            });
        }

        app.UseResponseCompression();
        app.UseRouting();
        app.UseIpRateLimiting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCors("CorsPolicy");
        app.UseWebSockets();
        app.MapMetrics();
        app.MapControllers();

        Console.WriteLine("Configuração de API finalizada com sucesso!");

        app.Run();
    }
}