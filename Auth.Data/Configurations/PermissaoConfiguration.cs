using Auth.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Data.Configurations;

public class PermissaoConfiguration : IEntityTypeConfiguration<Permissao>
{

    public void Configure(EntityTypeBuilder<Permissao> modelBuilder)
    {
        modelBuilder.ConfigureBaseEntity();

        modelBuilder.ToTable("permissao");
    }
}