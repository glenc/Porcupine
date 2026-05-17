using Microsoft.AspNetCore.Http.HttpResults;
using Porcupine.Application.Triggers.Queries.ListTriggers;

namespace Porcupine.WebApi.Endpoints;

public class Triggers : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(this)
            .RequireAuthorization();

        group
            .MapGet(ListTriggers);
        
        return group;
    }

    public async Task<Ok<TriggerListVm>> ListTriggers(ISender sender)
    {
        var result = await sender.Send(new ListTriggersQuery { });
        return TypedResults.Ok(result);
    }
}