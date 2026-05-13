using Porcupine.Application.Common.Interfaces;
using Porcupine.Application.Common.Security;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.Organizations.Queries.GetOrganization;

[Authorize]
public record GetOrganizationQuery : IRequest<OrganizationDetailVm>
{
    public int Id { get; init; }
}

public class GetOrganizationQueryValidator : AbstractValidator<GetOrganizationQuery>
{
    public GetOrganizationQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetOrganizationQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetOrganizationQuery, OrganizationDetailVm>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<OrganizationDetailVm> Handle(GetOrganizationQuery request, CancellationToken cancellationToken)
    {
        if (await _context.Organizations.AnyAsync(x => x.Id == request.Id, cancellationToken) == false)
            throw new NotFoundException("Id", nameof(Organization));
        
        var entity = await _context.Organizations.Where(x => x.Id == request.Id)
            .Include(x => x.LifecycleStage)
            .Include(x => x.Industry)
            .Include(x => x.MarketSegment)
            .AsNoTracking()
            .FirstAsync(cancellationToken);
        
        return _mapper.Map<OrganizationDetailVm>(entity);
    }
}
