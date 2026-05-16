// using Porcupine.Application.Common.Interfaces;
// using Porcupine.Domain.Entities;
// using Porcupine.Domain.Triggers;

// namespace Porcupine.Application.Common.NotificationHandlers;

// public class MarketSegmentEventTriggerHandler<TNotification>(IApplicationDbContext context) 
//     : EventTriggerHandlerBase<MarketSegment>(context)
//     , INotificationHandler<TNotification> where TNotification : IDomainEventTrigger<MarketSegment>
// {
//     public async Task Handle(TNotification notification, CancellationToken cancellationToken)
//     {
//         await HandleTrigger(notification);
//     }
// }