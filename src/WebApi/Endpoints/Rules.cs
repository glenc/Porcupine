using Microsoft.AspNetCore.Http.HttpResults;
using Porcupine.Application.Rules.Commands.CreateRule;
using Porcupine.Application.Rules.Queries.ListRules;

namespace Porcupine.WebApi.Endpoints;

public class Rules : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(this)
            .RequireAuthorization();

        group
            .MapGet(ListRules)
            //.MapGet(Get, "{rule_id}")
            .MapPost(CreateRule);
            //.MapPut(Update, "{rule_id}");
        
        return group;
    }

    public async Task<Ok<RuleListVm>> ListRules(ISender sender)
    {
        var result = await sender.Send(new ListRulesQuery { });
        return TypedResults.Ok(result);
    }

    public async Task<Ok<object>> Get(ISender sender, int rule_id)
    {
        await Task.Yield();
        throw new NotImplementedException();

        // var result = null;
        // return TypedResults.Ok(result);
    }

    public async Task<Created<int>> CreateRule(ISender sender, CreateRuleCommand command)
    {
        var result = await sender.Send(command);
        return TypedResults.Created($"/{nameof(Rules)}/{result}", result);
    }

    public async Task<Results<NoContent, BadRequest>> Update(ISender sender, int rule_id, object command)
    {
        await Task.Yield();
        throw new NotImplementedException();
        
        // if (rule_id != command.Id) return TypedResults.BadRequest();

        // await sender.Send(command);

        // return TypedResults.NoContent();
    }
}