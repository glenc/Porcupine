using Porcupine.Infrastructure.Identity;

namespace Porcupine.WebApi.Endpoints;

public class Users : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(this);
        group.MapIdentityApi<ApplicationUser>();

        return group;
    }
}
