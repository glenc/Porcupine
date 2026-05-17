using Microsoft.AspNetCore.Http.HttpResults;
using Porcupine.Application.Rules.Commands.CreateRuleAction;

namespace Porcupine.WebApi.Endpoints;

[NestedUnder(typeof(Rules), "{rule_id}")]
public class Actions : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(this)
            .RequireAuthorization();

        group
            .MapPost(CreateAction);
        
        return group;
    }

    public async Task<Results<Created<int>, BadRequest>> CreateAction(int rule_id, ISender sender, CreateRuleActionCommand command)
    {
        if (rule_id != command.RuleId) return TypedResults.BadRequest();

        var result = await sender.Send(command);
        return TypedResults.Created($"/{nameof(Rules)}/{rule_id}/{nameof(Actions)}/{result}", result);
    }

    public async Task<Results<NoContent, BadRequest>> Update(ISender sender, int name, object command)
    {
        await Task.Yield();
        throw new NotImplementedException();
        
        // if (name != command.Id) return TypedResults.BadRequest();

        // await sender.Send(command);

        // return TypedResults.NoContent();
    }
}