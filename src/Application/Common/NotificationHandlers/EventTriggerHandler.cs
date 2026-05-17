using Porcupine.Application.Common.Interfaces;
using Porcupine.Domain.Common;
using Porcupine.Domain.Entities;
using Porcupine.Domain.Events;
using Porcupine.Domain.Triggers;
using Porcupine.Application.Industries.Commands.CreateIndustry;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;

namespace Porcupine.Application.Common.NotificationHandlers;

//works for industry
public abstract class EventTriggerHandler<TEntity>(IApplicationDbContext context, ISender sender) where TEntity : BaseEntity
{
    private readonly IApplicationDbContext _context = context;

    protected async Task HandleTrigger(IDomainEventTrigger<TEntity> trigger)
    {
        var triggerName = trigger.GetType().AssemblyQualifiedName;

        // find rules matching trigger type
        var rules = await _context.Rules
            .Where(x => x.TriggerType == TriggerType.DomainEvent && x.TriggerName == triggerName)
            .AsNoTracking()
            .ToListAsync(CancellationToken.None);

        foreach (var rule in rules)
        {
            // check criteria
            if (MatchesFilter(trigger.Entity, rule.Criteria))
            {
                Console.WriteLine($"TRIGGER {trigger.GetType().Name} for {trigger.Entity.GetType()}.{trigger.Entity.Id}");
                await sender.Send(new FakeAction(), CancellationToken.None);
            }
        }
    }

    private static bool MatchesFilter(object obj, string? filter)
    {
        if (filter == null)
            return true;
        
        var parameter = Expression.Parameter(obj.GetType(), "x");

        var lambda = DynamicExpressionParser.ParseLambda(
            new[] { parameter },
            typeof(bool),
            filter
        );

        return (bool)lambda.Compile().DynamicInvoke(obj)!;
    }
}

public record FakeAction : IRequest { }