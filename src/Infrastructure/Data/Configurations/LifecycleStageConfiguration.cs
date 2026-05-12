using Porcupine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Porcupine.Infrastructure.Data.Configurations;

public class LifecycleStageConfiguration : IEntityTypeConfiguration<LifecycleStage>
{
    public void Configure(EntityTypeBuilder<LifecycleStage> builder)
    {
        builder.Property(x => x.Name)
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(x => x.Order)
            .IsRequired();
    }
}