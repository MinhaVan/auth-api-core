using System.Diagnostics.CodeAnalysis;
using Auth.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Data.Configurations;

[ExcludeFromCodeCoverage]
public class AlunoConfiguration : IEntityTypeConfiguration<Aluno>
{
    public void Configure(EntityTypeBuilder<Aluno> modelBuilder)
    {
        modelBuilder.ConfigureBaseEntity();
        modelBuilder.ToTable("aluno");

        modelBuilder.HasOne(x => x.Responsavel)
            .WithMany(y => y.Alunos)
            .HasForeignKey(x => x.ResponsavelId);

        modelBuilder.HasOne(x => x.Empresa)
            .WithMany(y => y.Alunos)
            .HasForeignKey(x => x.EmpresaId);

        modelBuilder.HasOne(x => x.EnderecoPartida)
            .WithMany(y => y.EnderecosPartidas)
            .HasForeignKey(x => x.EnderecoPartidaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.HasOne(x => x.EnderecoRetorno)
            .WithMany(y => y.EnderecosRetornos)
            .HasForeignKey(x => x.EnderecoRetornoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.HasOne(x => x.EnderecoDestino)
            .WithMany(y => y.EnderecosDestinos)
            .HasForeignKey(x => x.EnderecoDestinoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Remover esta linha
        // modelBuilder.Ignore(y => y.EnderecoRetorno);
    }
}
