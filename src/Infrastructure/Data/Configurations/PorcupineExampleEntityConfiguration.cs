using Porcupine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Porcupine.Infrastructure.Data.Configurations;

public class PorcupineExampleEntityConfiguration : IEntityTypeConfiguration<PorcupineExampleEntity>
{
    public void Configure(EntityTypeBuilder<PorcupineExampleEntity> builder)
    {
        builder.Property(x => x.Name)
            .HasMaxLength(255)
            .IsRequired();
    }
}