using Microsoft.AspNetCore.Http.HttpResults;
using Porcupine.Application.Industries.Commands.CreateIndustry;
using Porcupine.Application.Industries.Commands.UpdateIndustry;
using Porcupine.Application.Industries.Queries.GetIndustry;
using Porcupine.Application.Industries.Queries.ListIndustries;

namespace Porcupine.WebApi.Endpoints;

public class Industries : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(this)
            .RequireAuthorization();

        group
            .MapGet(ListIndustries)
            .MapGet(GetIndustry, "{industry_id}")
            .MapPost(CreateIndustry)
            .MapPut(UpdateIndustry, "{industry_id}");
        
        return group;
    }

    public async Task<Ok<IndustryListVm>> ListIndustries(ISender sender)
    {
        var result = await sender.Send(new ListIndustriesQuery());
        return TypedResults.Ok(result);
    }

    public async Task<Ok<IndustryDetailVm>> GetIndustry(ISender sender, int industry_id)
    {
        var result = await sender.Send(new GetIndustryQuery { Id = industry_id });
        return TypedResults.Ok(result);
    }

    public async Task<Created<int>> CreateIndustry(ISender sender, CreateIndustryCommand command)
    {
        var result = await sender.Send(command);
        return TypedResults.Created($"/{nameof(Industries)}/{result}", result);
    }

    public async Task<Results<NoContent, BadRequest>> UpdateIndustry(ISender sender, int industry_id, UpdateIndustryCommand command)
    {
        if (industry_id != command.Id) return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }
}