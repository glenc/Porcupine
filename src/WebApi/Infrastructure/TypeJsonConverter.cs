using System.Text.Json;
using System.Text.Json.Serialization;

namespace Porcupine.WebApi.Infrastructure;

public class TypeJsonConverter : JsonConverter<Type>
{
    public override Type Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var typeName = reader.GetString();

        if (typeName is null)
            return null!;
        
        return Type.GetType(typeName)
            ?? throw new JsonException($"Unable to resolve type: {typeName}");
    }

    public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.AssemblyQualifiedName);
    }
}