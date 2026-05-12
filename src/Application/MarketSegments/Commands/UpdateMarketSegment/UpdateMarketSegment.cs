using Porcupine.Application.Common.Interfaces;
using Porcupine.Application.Common.Security;
using Porcupine.Domain.Constants;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.MarketSegments.Commands.UpdateMarketSegment;

[Authorize(Roles = Roles.Administrator)]
public record UpdateMarketSegmentCommand : IRequest<int>
{
    public int Id { get; init; }
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
}

public class UpdateMarketSegmentCommandValidator : AbstractValidator<UpdateMarketSegmentCommand>
{
    public UpdateMarketSegmentCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class UpdateMarketSegmentCommandHandler(IApplicationDbContext context, IUser user) : IRequestHandler<UpdateMarketSegmentCommand, int>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IUser _user = user;

    public async Task<int> Handle(UpdateMarketSegmentCommand request, CancellationToken cancellationToken)
    {
        var segment = await _context.MarketSegments.FindAsync([request.Id], cancellationToken)
            ?? throw new NotFoundException("Id", nameof(MarketSegment));
        
        segment.Update(request.Name, request.Description);

        var result = await _context.SaveChangesAsync(cancellationToken);

        return result;
    }
}