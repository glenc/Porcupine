using Porcupine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Porcupine.Infrastructure.Data.Configurations;

public class MarketSegmentConfiguration : IEntityTypeConfiguration<MarketSegment>
{
    public void Configure(EntityTypeBuilder<MarketSegment> builder)
    {
        builder.Property(x => x.Name)
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(x => x.Description)
            .HasMaxLength(255);
    }
}