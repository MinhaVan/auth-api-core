using Auth.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Data.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> modelBuilder)
    {
        modelBuilder.ConfigureBaseEntity();
        modelBuilder.ToTable("usuario");
        modelBuilder.HasOne(x => x.Empresa)
            .WithMany(y => y.Usuarios)
            .HasForeignKey(x => x.EmpresaId);

        modelBuilder.HasMany(x => x.Enderecos)
            .WithOne(x => x.Usuario)
            .HasForeignKey(x => x.UsuarioId);

        modelBuilder.HasOne(x => x.EnderecoPrincipal)
            .WithMany()
            .HasForeignKey(x => x.EnderecoPrincipalId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}