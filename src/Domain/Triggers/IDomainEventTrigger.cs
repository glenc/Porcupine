using MediatR;

namespace Porcupine.Domain.Triggers;

public interface IDomainEventTrigger<TEntity> : ITrigger, INotification where TEntity : BaseEntity
{
    public TEntity Entity { get; }
}