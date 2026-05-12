using Porcupine.Application.Common.Interfaces;
using Porcupine.Application.Common.Security;
using Porcupine.Domain.Constants;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.MarketSegments.Commands.CreateMarketSegment;

[Authorize(Roles = Roles.Administrator)]
public record CreateMarketSegmentCommand : IRequest<int>
{
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
}

public class CreateMarketSegmentCommandValidator : AbstractValidator<CreateMarketSegmentCommand>
{
    public CreateMarketSegmentCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class CreateMarketSegmentCommandHandler(IApplicationDbContext context, IUser user) : IRequestHandler<CreateMarketSegmentCommand, int>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IUser _user = user;

    public async Task<int> Handle(CreateMarketSegmentCommand request, CancellationToken cancellationToken)
    {
        var segment = new MarketSegment(request.Name, request.Description);

        _context.MarketSegments.Add(segment);
        await _context.SaveChangesAsync(cancellationToken);

        return segment.Id;
    }
}