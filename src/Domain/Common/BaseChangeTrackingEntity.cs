using System.Linq.Expressions;
using Porcupine.Domain.Events;

namespace Porcupine.Domain.Common;

public abstract class BaseChangeTrackingEntity : BaseAuditableEntity
{
    private readonly Dictionary<string, object?> _changes = [];

    protected void ApplyChange<T>(Expression<Func<T>> getter, Action<T> setter, T newValue)
    {
        var body = getter.Body as MemberExpression;
        if (body == null && getter.Body is UnaryExpression unary)
        {
            body = unary.Operand as MemberExpression;
        }

        var propName = body?.Member.Name ?? "Unknown";

        var compiledGetter = getter.Compile();
        var currentValue = compiledGetter();

        if (!EqualityComparer<T>.Default.Equals(currentValue, newValue))
        {
            _changes.Add(propName, currentValue);
            setter(newValue);
        }
    }

    protected bool HasChanges()
    {
        return _changes.Count > 0;
    }

    protected IDictionary<string, object?> GetAndClearChanges()
    {
        var copy = new Dictionary<string, object?>(_changes);
        _changes.Clear();
        return copy;
    }
}
