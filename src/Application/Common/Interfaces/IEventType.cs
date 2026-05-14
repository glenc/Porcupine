namespace Porcupine.Application.Common.Interfaces;

public interface IEventType
{
    Type Type { get; }
    string TypeName { get; }
    string DisplayName { get; }
    string Description { get; }
}