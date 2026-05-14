using Microsoft.AspNetCore.Http.HttpResults;
using Porcupine.Application.EventTypes.Queries.ListEventTypes;

namespace Porcupine.WebApi.Endpoints;

public class EventTypes : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(this)
            .RequireAuthorization();

        group
            .MapGet(ListEventTypes);
        
        return group;
    }

    public async Task<Ok<EventTypeListVm>> ListEventTypes(ISender sender)
    {
        var result = await sender.Send(new ListEventTypesQuery { });
        return TypedResults.Ok(result);
    }
}