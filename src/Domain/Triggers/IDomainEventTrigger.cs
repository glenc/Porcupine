using MediatR;

namespace Porcupine.Domain.Triggers;

public interface IDomainEventTrigger<TEntity> : ITrigger where TEntity : BaseEntity
{
    public TEntity Entity { get; }
}