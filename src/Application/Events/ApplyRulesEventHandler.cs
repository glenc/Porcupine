using MediatR;
using Porcupine.Application.Common.Interfaces;
using Porcupine.Application.Industries.Commands.CreateIndustry;
using Porcupine.Domain.Entities;
using Porcupine.Domain.Enums;

namespace Porcupine.Application.Events;

public class ApplyRulesEventHandler<TNotification>(IEventTypeService eventTypeService, IApplicationDbContext context, ISender sender) : INotificationHandler<TNotification>
    where TNotification : INotification
{
    private readonly IEventTypeService _eventTypeService = eventTypeService;
    private readonly IApplicationDbContext _context = context;
    private readonly ISender _sender = sender;

    public async Task Handle(TNotification notification, CancellationToken cancellationToken)
    {
        var eventType = notification.GetType();

        if (_eventTypeService.IsTypeRegistered(eventType) == true)
        {
            // load matching rules from db
            var rules = await _context.Rules
                .Where(x => x.TriggerType == TriggerType.DomainEvent && x.TriggerName == eventType.AssemblyQualifiedName)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            
            // foreach (var rule in rules)
            // {
            //     await _sender.Send(new CreateIndustryCommand { Name = "New from trigger" }, cancellationToken);
            // }
        }
    }
}