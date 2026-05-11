namespace Porcupine.WebApi.Infrastructure;

public interface IEndpointGroup
{
    RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder app);
}