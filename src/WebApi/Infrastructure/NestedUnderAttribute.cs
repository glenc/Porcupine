using System.Diagnostics.CodeAnalysis;

namespace Porcupine.WebApi.Infrastructure;

[AttributeUsage(AttributeTargets.Class)]
public class NestedUnderAttribute(Type parentType, [StringSyntax("Route")] string pattern = "") : Attribute
{
    public Type ParentType { get; } = parentType;
    public string Pattern { get; } = pattern;
}