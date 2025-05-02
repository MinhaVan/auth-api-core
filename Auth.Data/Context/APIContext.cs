using Microsoft.EntityFrameworkCore;
using Auth.Domain.Models;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Threading;
using Auth.Data.Configurations;
using System.Diagnostics.CodeAnalysis;

namespace Auth.Data.Context;

[ExcludeFromCodeCoverage]
public class APIContext : DbContext
{
    public APIContext(DbContextOptions<APIContext> options) : base(options)
    { }

    public override int SaveChanges()
    {
        AtualizarDatas();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AtualizarDatas();
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
        modelBuilder.ApplyConfiguration(new EmpresaConfiguration());
        modelBuilder.ApplyConfiguration(new MotoristaConfiguration());
        modelBuilder.ApplyConfiguration(new EnderecoConfiguration());
        modelBuilder.ApplyConfiguration(new PermissaoConfiguration());
        modelBuilder.ApplyConfiguration(new AlunoConfiguration());
        modelBuilder.ApplyConfiguration(new UsuarioPermissaoConfiguration());

        base.OnModelCreating(modelBuilder);
    }
    private void AtualizarDatas()
    {
        var now = DateTime.UtcNow;
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is Entity &&
                        (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                ((Entity)entry.Entity).DataCriacao = now;
                ((Entity)entry.Entity).Status = Domain.Enums.StatusEntityEnum.Ativo;
            }
            ((Entity)entry.Entity).DataAlteracao = now;
        }
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Aluno> Alunos { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }
    public DbSet<Motorista> Motoristas { get; set; }
    public DbSet<Permissao> Permissoes { get; set; }
    public DbSet<UsuarioPermissao> UsuarioPermissoes { get; set; }
}