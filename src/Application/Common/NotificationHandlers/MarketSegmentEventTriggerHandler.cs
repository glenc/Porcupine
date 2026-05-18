using Porcupine.Application.Common.Interfaces;
using Porcupine.Domain.Entities;
using Porcupine.Domain.Triggers;

namespace Porcupine.Application.Common.NotificationHandlers;

public class MarketSegmentEventTriggerHandler<TNotification>(IApplicationDbContext context, ISender sender) 
    : EventTriggerHandler<MarketSegment>(context, sender)
    , INotificationHandler<TNotification> where TNotification : IDomainEventTrigger<MarketSegment>
{
    public async Task Handle(TNotification notification, CancellationToken cancellationToken)
    {
        await HandleTrigger(notification, cancellationToken);
    }
}