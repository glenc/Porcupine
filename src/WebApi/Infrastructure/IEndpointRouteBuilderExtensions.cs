using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Porcupine.WebApi.Infrastructure;

public static class IEndpointRouteBuilderExtensions
{
    public static void MapAllEndpoints(this IEndpointRouteBuilder app)
    {
        var types = typeof(Program).Assembly.GetTypes()
            .Where(t => typeof(IEndpointGroup).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .ToList();
        
        // identify root modules
        var rootTypes = types.Where(t => t.GetCustomAttribute<NestedUnderAttribute>() == null);

        foreach (var rootType in rootTypes)
        {
            var rootModule = (IEndpointGroup)Activator.CreateInstance(rootType)!;
            var group = rootModule.MapEndpoints(app);

            // find children
            MapChildren(group, rootType, types);
        }
    }

    private static void MapChildren(IEndpointRouteBuilder parentGroup, Type parentType, List<Type> allTypes)
    {
        var childTypes = allTypes.Where(t => t.GetCustomAttribute<NestedUnderAttribute>()?.ParentType == parentType);

        foreach (var childType in childTypes)
        {
            var childModule = (IEndpointGroup)Activator.CreateInstance(childType)!;
            var childGroup = childModule.MapEndpoints(parentGroup);

            // next level down
            MapChildren(childGroup, childType, allTypes);
        }
    }

    public static RouteGroupBuilder MapGroup(this IEndpointRouteBuilder builder, IEndpointGroup endpoint, [StringSyntax("Route")] string pattern = "")
    {
        var groupName = endpoint.GetType().Name;
        var parentPattern = endpoint.GetType().GetCustomAttribute<NestedUnderAttribute>()?.Pattern;

        var path = string.Join('/', [parentPattern, groupName.ToKebabCase()]);

        return builder.MapGroup(path)
            .WithTags(groupName);
    }

    public static IEndpointRouteBuilder MapGet(this IEndpointRouteBuilder builder, Delegate handler, [StringSyntax("Route")] string pattern = "")
    {
        Guard.Against.AnonymousMethod(handler);

        builder.MapGet(pattern, handler)
            .WithName(handler.Method.Name);

        return builder;
    }

    public static IEndpointRouteBuilder MapPost(this IEndpointRouteBuilder builder, Delegate handler, [StringSyntax("Route")] string pattern = "")
    {
        Guard.Against.AnonymousMethod(handler);

        builder.MapPost(pattern, handler)
            .WithName(handler.Method.Name);

        return builder;
    }

    public static IEndpointRouteBuilder MapPut(this IEndpointRouteBuilder builder, Delegate handler, [StringSyntax("Route")] string pattern)
    {
        Guard.Against.AnonymousMethod(handler);

        builder.MapPut(pattern, handler)
            .WithName(handler.Method.Name);

        return builder;
    }

    public static IEndpointRouteBuilder MapDelete(this IEndpointRouteBuilder builder, Delegate handler, [StringSyntax("Route")] string pattern)
    {
        Guard.Against.AnonymousMethod(handler);

        builder.MapDelete(pattern, handler)
            .WithName(handler.Method.Name);

        return builder;
    }
}
