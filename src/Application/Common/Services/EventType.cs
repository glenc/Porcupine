using Porcupine.Application.Common.Interfaces;

namespace Porcupine.Application.Common.Services;

public record EventType(Type Type, string DisplayName, string Description) : IEventType 
{
    public string TypeName => Type.AssemblyQualifiedName ?? "";
}