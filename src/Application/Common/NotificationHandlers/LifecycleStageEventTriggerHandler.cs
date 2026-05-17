using Porcupine.Application.Common.Interfaces;
using Porcupine.Domain.Entities;
using Porcupine.Domain.Triggers;

namespace Porcupine.Application.Common.NotificationHandlers;

public class LifecycleStageEventTriggerHandler<TNotification>(IApplicationDbContext context, ISender sender) 
    : EventTriggerHandler<LifecycleStage>(context, sender)
    , INotificationHandler<TNotification> where TNotification : IDomainEventTrigger<LifecycleStage>
{
    public async Task Handle(TNotification notification, CancellationToken cancellationToken)
    {
        await HandleTrigger(notification);
    }
}