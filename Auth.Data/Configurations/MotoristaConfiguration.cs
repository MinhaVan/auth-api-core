using Auth.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Data.Configurations;

public class MotoristaConfiguration : IEntityTypeConfiguration<Motorista>
{
    public void Configure(EntityTypeBuilder<Motorista> modelBuilder)
    {
        modelBuilder.ConfigureBaseEntity();
        modelBuilder.ToTable("motorista");
        modelBuilder.HasOne(x => x.Usuario)
            .WithOne(y => y.Motorista)
            .HasForeignKey<Motorista>(x => x.UsuarioId);
    }
}