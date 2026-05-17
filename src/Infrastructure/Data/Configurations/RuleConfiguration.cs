using Porcupine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;
using Porcupine.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
        
        builder.Ignore(x => x.Actions);
        
        var serializationOptions = new JsonSerializerOptions();
        builder.Property<List<RuleAction>>("_actions")
            .HasColumnName("Actions")
            .HasConversion(
                v => JsonSerializer.Serialize(v, serializationOptions),
                v => JsonSerializer.Deserialize<List<RuleAction>>(v, serializationOptions)!
            )
            .Metadata
            .SetValueComparer(
                new ValueComparer<List<RuleAction>>(
                    (c1, c2) => JsonSerializer.Serialize(c1, serializationOptions) ==
                                JsonSerializer.Serialize(c2, serializationOptions),

                    c => c == null ? 0 : JsonSerializer.Serialize(c, serializationOptions).GetHashCode(),

                    c => JsonSerializer.Deserialize<List<RuleAction>>(
                        JsonSerializer.Serialize(c, serializationOptions),
                        serializationOptions)!
                    )
                );
    }
}