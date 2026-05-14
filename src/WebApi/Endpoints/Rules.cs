using Microsoft.AspNetCore.Http.HttpResults;
using Porcupine.Application.Rules.Commands.CreateRule;
using Porcupine.Application.Rules.Commands.UpdateRule;
using Porcupine.Application.Rules.Queries.GetRule;
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
            .MapGet(GetRule, "{rule_id}")
            .MapPost(CreateRule)
            .MapPut(UpdateRule, "{rule_id}");
        
        return group;
    }

    public async Task<Ok<RuleListVm>> ListRules(ISender sender)
    {
        var result = await sender.Send(new ListRulesQuery { });
        return TypedResults.Ok(result);
    }

    public async Task<Ok<RuleDetailVm>> GetRule(ISender sender, int rule_id)
    {
        var result = await sender.Send(new GetRuleQuery { Id = rule_id });
        return TypedResults.Ok(result);
    }

    public async Task<Created<int>> CreateRule(ISender sender, CreateRuleCommand command)
    {
        var result = await sender.Send(command);
        return TypedResults.Created($"/{nameof(Rules)}/{result}", result);
    }

    public async Task<Results<NoContent, BadRequest>> UpdateRule(ISender sender, int rule_id, UpdateRuleCommand command)
    {
        if (rule_id != command.Id) return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }
}