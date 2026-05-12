using Microsoft.AspNetCore.Http.HttpResults;
using Porcupine.Application.MarketSegments.Commands.CreateMarketSegment;
using Porcupine.Application.MarketSegments.Queries.GetMarketSegment;
using Porcupine.Application.MarketSegments.Queries.ListMarketSegments;

namespace Porcupine.WebApi.Endpoints;

public class MarketSegments : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(this)
            .RequireAuthorization();

        group
            .MapGet(ListMarketSegments)
            .MapGet(GetMarketSegment, "{segment_id}")
            .MapPost(CreateMarketSegment);
            //.MapPut(UpdateMarketSegment, "{segment_id}");
        
        return group;
    }

    public async Task<Ok<MarketSegmentListVm>> ListMarketSegments(ISender sender)
    {
        var result = await sender.Send(new ListMarketSegmentsQuery());
        return TypedResults.Ok(result);
    }

    public async Task<Ok<MarketSegmentDetailVm>> GetMarketSegment(ISender sender, int segment_id)
    {
        var result = await sender.Send(new GetMarketSegmentQuery { Id = segment_id });
        return TypedResults.Ok(result);
    }

    public async Task<Created<int>> CreateMarketSegment(ISender sender, CreateMarketSegmentCommand command)
    {
        var result = await sender.Send(command);
        return TypedResults.Created($"/{nameof(MarketSegments)}/{result}", result);
    }

    // public async Task<Results<NoContent, BadRequest>> UpdateMarketSegment(ISender sender, int segment_id, object command)
    // {
    //     await Task.Yield();
    //     throw new NotImplementedException();
        
    //     // if (MarketSegment_id != command.Id) return TypedResults.BadRequest();

    //     // await sender.Send(command);

    //     // return TypedResults.NoContent();
    // }
}