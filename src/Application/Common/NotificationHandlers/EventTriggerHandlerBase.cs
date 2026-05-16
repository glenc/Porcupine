using Porcupine.Application.Common.Interfaces;
using Porcupine.Domain.Common;
using Porcupine.Domain.Entities;
using Porcupine.Domain.Events;
using Porcupine.Domain.Triggers;

namespace Porcupine.Application.Common.NotificationHandlers;

//works for industry
public abstract class EventTriggerHandlerBase<TEntity>(IApplicationDbContext context, ITriggerService triggerService) where TEntity : BaseEntity
{
    private readonly IApplicationDbContext _context = context;
    private readonly ITriggerService _triggerService = triggerService;

    protected async Task HandleTrigger(IDomainEventTrigger<TEntity> trigger)
    {
        Console.WriteLine($"TRIGGER: {trigger.GetType().Name}");

        var eventType = trigger.GetType();
        
        // if (_triggerService.IsTypeRegistered(eventType) == true)
        // {
        //     // load matching rules from db
        //     var rules = await _context.Rules
        //         .Where(x => x.TriggerType == TriggerType.DomainEvent && x.TriggerName == eventType.AssemblyQualifiedName)
        //         .AsNoTracking()
        //         .ToListAsync(cancellationToken);
            
        //     foreach (var rule in rules)
        //     {
        //         Console.WriteLine($"Rule {rule.Name} triggered by {eventType.Name}");
        //         //await _sender.Send(new CreateIndustryCommand { Name = "New from trigger" }, cancellationToken);
        //     }
        // }
    }
}