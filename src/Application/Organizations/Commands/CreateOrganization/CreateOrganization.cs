using Porcupine.Application.Common.Interfaces;
using Porcupine.Application.Common.Security;
using Porcupine.Domain.Constants;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.Organizations.Commands.CreateOrganization;

[Authorize(Roles = Roles.Administrator)]
public record CreateOrganizationCommand : IRequest<int>
{
    public string Name { get; init; } = "";
    public int LifecycleStageId { get; init; }
    public int? IndustryId { get; init; }
    public int? MarketSegmentId { get; init; }
}

public class CreateOrganizationCommandValidator : AbstractValidator<CreateOrganizationCommand>
{
    public CreateOrganizationCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.LifecycleStageId)
            .NotEmpty()
            .GreaterThan(0);
    }
}

public class CreateOrganizationCommandHandler(IApplicationDbContext context, IUser user) : IRequestHandler<CreateOrganizationCommand, int>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IUser _user = user;

    public async Task<int> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
    {
        var lifecycleStage = await _context.LifecycleStages.Where(x => x.Id == request.LifecycleStageId).FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("Id", nameof(LifecycleStage));

        var org = new Organization(request.Name, lifecycleStage);

        if (request.IndustryId != null)
        {
            var industry = await _context.Industries.Where(x => x.Id == request.IndustryId).FirstOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException("Id", nameof(Industry));
            
            org.SetIndustry(industry);
        }

        if (request.MarketSegmentId != null)
        {
            var segment = await _context.MarketSegments.Where(x => x.Id == request.MarketSegmentId).FirstOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException("Id", nameof(MarketSegment));
            
            org.SetMarketSegment(segment);
        }

        _context.Organizations.Add(org);
        await _context.SaveChangesAsync(cancellationToken);

        return org.Id;
    }
}