using Auth.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Data.Configurations;

public static class ModelBuilderExtensions
{
    public static EntityTypeBuilder<T> ConfigureBaseEntity<T>(this EntityTypeBuilder<T> builder) where T : Entity
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        return builder;
    }
}