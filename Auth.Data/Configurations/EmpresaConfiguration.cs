using System.Diagnostics.CodeAnalysis;
using Auth.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Data.Configurations;

[ExcludeFromCodeCoverage]
public class EmpresaConfiguration : IEntityTypeConfiguration<Empresa>
{
    public void Configure(EntityTypeBuilder<Empresa> modelBuilder)
    {
        modelBuilder.ConfigureBaseEntity();
        modelBuilder.ToTable("empresa");
        modelBuilder.HasMany(x => x.Usuarios)
            .WithOne(y => y.Empresa)
            .HasForeignKey(x => x.EmpresaId);

    }
}