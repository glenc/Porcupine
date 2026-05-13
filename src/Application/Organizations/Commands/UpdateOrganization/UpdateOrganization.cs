using Porcupine.Application.Common.Interfaces;
using Porcupine.Application.Common.Security;
using Porcupine.Domain.Constants;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.Organizations.Commands.UpdateOrganization;

[Authorize(Roles = Roles.Administrator)]
public record UpdateOrganizationCommand : IRequest<int>
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public int? LifecycleStageId { get; init; }
    public int? IndustryId { get; init; }
    public int? MarketSegmentId { get; init; }
}

public class UpdateOrganizationCommandValidator : AbstractValidator<UpdateOrganizationCommand>
{
    public UpdateOrganizationCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class UpdateOrganizationCommandHandler(IApplicationDbContext context, IUser user) : IRequestHandler<UpdateOrganizationCommand, int>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IUser _user = user;

    public async Task<int> Handle(UpdateOrganizationCommand request, CancellationToken cancellationToken)
    {
        var org = await _context.Organizations.FindAsync([request.Id], cancellationToken)
            ?? throw new NotFoundException("Id", nameof(Organization));
        
        if (request.Name != null)
            org.Update(request.Name);
        
        if (request.IndustryId != null)
        {
            var newIndustry = await _context.Industries.FindAsync([request.IndustryId], cancellationToken)
                ?? throw new NotFoundException("Id", nameof(Industry));

            org.SetIndustry(newIndustry);
        }

        if (request.MarketSegmentId != null)
        {
            var newSegment = await _context.MarketSegments.FindAsync([request.MarketSegmentId], cancellationToken)
                ?? throw new NotFoundException("Id", nameof(MarketSegment));

            org.SetMarketSegment(newSegment);
        }

        if (request.LifecycleStageId != null)
        {
            var newStage = await _context.LifecycleStages.FindAsync([request.LifecycleStageId], cancellationToken)
                ?? throw new NotFoundException("Id", nameof(LifecycleStage));

            org.ChangeLifecycleStage(newStage);
        }

        var result = await _context.SaveChangesAsync(cancellationToken);

        return result;
    }
}