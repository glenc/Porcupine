using System.Reflection;
using System.Text.Json;

namespace Porcupine.Application.Common.Services;

public static class CommandFactory
{
    public static object CreateCommand(Type commandType, object entity, string commandTemplate)
    {
        Guard.Against.NullOrWhiteSpace(commandTemplate);

        if (IsRequest(commandType) == false)
            throw new ArgumentException("CommandType does not implement IRequest<>");

        var result = commandTemplate;

        var properties = entity.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance);
        
        foreach (var prop in properties)
        {
            var token = $"{{{prop.Name}}}";
            var value = prop.GetValue(entity);

            string replacement = value switch
            {
                null => "null",
                //string s => JsonSerializer.Serialize(s),
                string s => s,
                _ => JsonSerializer.Serialize(value)
            };

            result = result.Replace(token, replacement);
        }

        return JsonSerializer.Deserialize(result, commandType)!;
    }

    private static bool IsRequest(Type type)
    {
        return typeof(IRequest).IsAssignableFrom(type)
            ||
            type.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IRequest<>));
    }
}