using Microsoft.AspNetCore.Http.HttpResults;
using Porcupine.Application.LifecycleStages.Commands.CreateLifecycleStage;
using Porcupine.Application.LifecycleStages.Commands.UpdateLifecycleStage;
using Porcupine.Application.LifecycleStages.Queries.GetLifecycleStage;
using Porcupine.Application.LifecycleStages.Queries.ListLifecycleStages;

namespace Porcupine.WebApi.Endpoints;

public class LifecycleStages : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(this)
            .RequireAuthorization();

        group
            .MapGet(ListLifecycleStages)
            .MapGet(GetLifecycleStage, "{stage_id}")
            .MapPost(CreateLifecycleStage)
            .MapPut(UpdateLifecycleStage, "{stage_id}");
        
        return group;
    }

    public async Task<Ok<LifecycleStageListVm>> ListLifecycleStages(ISender sender)
    {
        var result = await sender.Send(new ListLifecycleStagesQuery());
        return TypedResults.Ok(result);
    }

    public async Task<Ok<LifecycleStageDetailVm>> GetLifecycleStage(ISender sender, int stage_id)
    {
        var result = await sender.Send(new GetLifecycleStageQuery { Id = stage_id });
        return TypedResults.Ok(result);
    }

    public async Task<Created<int>> CreateLifecycleStage(ISender sender, CreateLifecycleStageCommand command)
    {
        var result = await sender.Send(command);
        return TypedResults.Created($"/{nameof(LifecycleStages)}/{result}", result);
    }

    public async Task<Results<NoContent, BadRequest>> UpdateLifecycleStage(ISender sender, int stage_id, UpdateLifecycleStageCommand command)
    {
       if (stage_id != command.Id) return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }
}