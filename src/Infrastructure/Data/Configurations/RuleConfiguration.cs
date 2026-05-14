using Porcupine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Porcupine.Infrastructure.Data.Configurations;

public class RuleConfiguration : IEntityTypeConfiguration<Rule>
{
    public void Configure(EntityTypeBuilder<Rule> builder)
    {
        builder.Property(x => x.Name)
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(x => x.TriggerName)
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(x => x.TriggerType)
            .IsRequired();
        
        builder.Property(x => x.Criteria)
            .HasMaxLength(255);
    }
}