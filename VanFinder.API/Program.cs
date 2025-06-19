// src/Web/Program.cs
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VanFinder.Application.Implementation;
using VanFinder.Data.Context;
using VanFinder.Data.Implementation;
using VanFinder.Domain.Interfaces.Application;

var builder = WebApplication.CreateBuilder(args);

// Configura EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Injeta serviços
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<AuthRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
