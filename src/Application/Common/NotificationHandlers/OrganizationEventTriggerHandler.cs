using Porcupine.Application.Common.Interfaces;
using Porcupine.Domain.Entities;
using Porcupine.Domain.Triggers;

namespace Porcupine.Application.Common.NotificationHandlers;

public class OrganizationEventTriggerHandler<TNotification>(IApplicationDbContext context, ISender sender) 
    : EventTriggerHandler<Organization>(context, sender)
    , INotificationHandler<TNotification> where TNotification : IDomainEventTrigger<Organization>
{
    public async Task Handle(TNotification notification, CancellationToken cancellationToken)
    {
        await HandleTrigger(notification, cancellationToken);
    }
}