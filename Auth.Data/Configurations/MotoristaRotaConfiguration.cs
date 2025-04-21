using Auth.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Data.Configurations;

public class MotoristaRotaConfiguration : IEntityTypeConfiguration<MotoristaRota>
{
    public void Configure(EntityTypeBuilder<MotoristaRota> modelBuilder)
    {
        modelBuilder.HasKey(x => new { x.MotoristaId, x.RotaId });
        modelBuilder.ToTable("motorista_rota");

        modelBuilder.HasOne(x => x.Motorista)
            .WithMany(y => y.MotoristaRotas)
            .HasForeignKey(x => x.MotoristaId);
    }
}