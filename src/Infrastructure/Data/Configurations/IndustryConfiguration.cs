using Porcupine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Porcupine.Infrastructure.Data.Configurations;

public class IndustryConfiguration : IEntityTypeConfiguration<Industry>
{
    public void Configure(EntityTypeBuilder<Industry> builder)
    {
        builder.Property(x => x.Name)
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(x => x.Description)
            .HasMaxLength(255);
    }
}