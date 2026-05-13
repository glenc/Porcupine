using Microsoft.AspNetCore.Http.HttpResults;
using Porcupine.Application.Organizations.Commands.CreateOrganization;
using Porcupine.Application.Organizations.Queries.GetOrganization;
using Porcupine.Application.Organizations.Queries.ListOrganizations;

namespace Porcupine.WebApi.Endpoints;

public class Organizations : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(this)
            .RequireAuthorization();

        group
            .MapGet(ListOrganizations)
            .MapGet(GetOrganization, "{org_id}")
            .MapPost(CreateOrganization);
            //.MapPut(Update, "{org_id}");
        
        return group;
    }

    public async Task<Ok<OrganizationListVm>> ListOrganizations(ISender sender)
    {
        var result = await sender.Send(new ListOrganizationsQuery());
        return TypedResults.Ok(result);
    }

    public async Task<Ok<OrganizationDetailVm>> GetOrganization(ISender sender, int org_id)
    {
        var result = await sender.Send(new GetOrganizationQuery { Id = org_id });
        return TypedResults.Ok(result);
    }

    public async Task<Created<int>> CreateOrganization(ISender sender, CreateOrganizationCommand command)
    {
        var result = await sender.Send(command);
        return TypedResults.Created($"/{nameof(Organizations)}/{result}", result);
    }

    public async Task<Results<NoContent, BadRequest>> Update(ISender sender, int org_id, object command)
    {
        await Task.Yield();
        throw new NotImplementedException();
        
        // if (org_id != command.Id) return TypedResults.BadRequest();

        // await sender.Send(command);

        // return TypedResults.NoContent();
    }
}