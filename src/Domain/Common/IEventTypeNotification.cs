using MediatR;

namespace Porcupine.Domain.Common;

public interface IEventTypeNotification : INotification
{
    
}

public interface IEventTypeNotification<TEntity> : INotification
{
    public TEntity Entity { get; }
}