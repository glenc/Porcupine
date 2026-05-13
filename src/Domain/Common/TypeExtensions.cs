namespace Porcupine.Domain.Common;

public static class TypeExtensions
{
    public static string GetFriendlyName(this Type type)
    {
        if(!type.IsGenericType)
            return type.FullName ?? type.Name;

        if (type.IsGenericParameter)
            return type.Name;
        
        if (type.IsArray)
            return $"{GetFriendlyName(type.GetElementType()!)}[]";
        
        if (Nullable.GetUnderlyingType(type) is Type nullableType)
            return $"{GetFriendlyName(nullableType)}?";
        
        var genericArgs = string.Join(", ",
            type.GetGenericArguments()
                .Select(GetFriendlyName));
            
        var name = type.Name;
        var tickIndex = name.IndexOf('`');

        if (tickIndex >= 0)
            name = name[..tickIndex];
        
        return $"{type.Namespace}.{name}<{genericArgs}>";
    }
}