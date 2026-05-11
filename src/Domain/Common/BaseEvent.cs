using MediatR;

namespace Porcupine.Domain.Common;

public abstract record BaseEvent : INotification
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
