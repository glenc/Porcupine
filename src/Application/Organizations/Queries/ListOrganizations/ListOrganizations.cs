using Porcupine.Application.Common.Interfaces;
using Porcupine.Application.Common.Mappings;
using Porcupine.Application.Common.Security;

namespace Porcupine.Application.Organizations.Queries.ListOrganizations;

[Authorize]
public record ListOrganizationsQuery : IRequest<OrganizationListVm>
{
}

public class ListOrganizationsQueryValidator : AbstractValidator<ListOrganizationsQuery>
{
    public ListOrganizationsQueryValidator()
    {
    }
}

public class ListOrganizationsQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<ListOrganizationsQuery, OrganizationListVm>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<OrganizationListVm> Handle(ListOrganizationsQuery request, CancellationToken cancellationToken)
    {
        var orgs = await _context.Organizations
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectToListAsync<OrganizationSummaryDto>(_mapper.ConfigurationProvider);

        return new OrganizationListVm(orgs);
    }
}
