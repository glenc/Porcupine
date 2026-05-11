using MediatR;

namespace Porcupine.Domain.Common;

public abstract class BaseEvent : INotification
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
