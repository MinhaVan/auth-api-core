using System.Diagnostics.CodeAnalysis;
using Auth.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Data.Configurations;

[ExcludeFromCodeCoverage]
public class UsuarioPermissaoConfiguration : IEntityTypeConfiguration<UsuarioPermissao>
{

    public void Configure(EntityTypeBuilder<UsuarioPermissao> modelBuilder)
    {
        modelBuilder.ConfigureBaseEntity();
        modelBuilder.HasKey(x => new { x.UsuarioId, x.PermissaoId });

        modelBuilder.ToTable("usuarioPermissao");
        modelBuilder.HasOne(x => x.Usuario)
            .WithMany(y => y.Permissoes)
            .HasForeignKey(x => x.UsuarioId);

        modelBuilder.HasOne(x => x.Permissao)
            .WithMany(y => y.Usuarios)
            .HasForeignKey(x => x.PermissaoId);
    }
}