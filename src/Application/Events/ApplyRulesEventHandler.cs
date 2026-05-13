using MediatR;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.Events;

// public class ApplyRulesEventHandler<TNotification> : INotificationHandler<TNotification>
//     where TNotification : INotification
// {
//     public Task Handle(TNotification notification, CancellationToken cancellationToken)
//     {
//         var eventType = notification.GetType().Name;

//         var rules = _rules.Where(x => x.EventName == eventType).ToList();

//         foreach (var rule in rules)
//         {
            
//         }

//         return Task.CompletedTask;
//     }

//     private readonly List<Rule> _rules = [
//         new Rule("Run Stage 1", "OrganizationCreatedEvent", "LifecycleStage.Id = 1"),
//         new Rule("Run Stage 2", "OrganizationCreatedEvent", "LifecycleStage.Id = 2"),
//         new Rule("Run Stage 3", "OrganizationCreatedEvent", "LifecycleStage.Id = 3"),
//         new Rule("Run always", "OrganizationCreatedEvent")
//     ];
// }