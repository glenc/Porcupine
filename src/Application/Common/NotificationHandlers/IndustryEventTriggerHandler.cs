using Porcupine.Application.Common.Interfaces;
using Porcupine.Domain.Entities;
using Porcupine.Domain.Triggers;

namespace Porcupine.Application.Common.NotificationHandlers;

public class IndustryEventTriggerHandler<TNotification>(IApplicationDbContext context, ITriggerService triggerService, ISender sender) 
    : EventTriggerHandler<Industry>(context, triggerService, sender)
    , INotificationHandler<TNotification> where TNotification : IDomainEventTrigger<Industry>
{
    public async Task Handle(TNotification notification, CancellationToken cancellationToken)
    {
        await HandleTrigger(notification);
    }
}