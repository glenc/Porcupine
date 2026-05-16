using Porcupine.Domain.Entities;
using Porcupine.Domain.Triggers;

namespace Porcupine.Application.Common.NotificationHandlers;

public class DomainEventNotificationHandler<TNotification>() : DomainEventNotificationHandlerBase<Industry>, 
    INotificationHandler<TNotification> where TNotification : IDomainEventTrigger<Industry>
{
    public async Task Handle(TNotification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"INDUSTRY NOTIFICATION {notification.GetType().Name}");
    }
}