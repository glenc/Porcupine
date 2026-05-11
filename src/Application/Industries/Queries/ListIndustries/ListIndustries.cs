using Porcupine.Application.Common.Interfaces;
using Porcupine.Application.Common.Mappings;
using Porcupine.Application.Common.Security;

namespace Porcupine.Application.Industries.Queries.ListIndustries;

[Authorize]
public record ListIndustriesQuery : IRequest<IndustryListVm>
{
}

public class ListIndustriesQueryValidator : AbstractValidator<ListIndustriesQuery>
{
    public ListIndustriesQueryValidator()
    {
    }
}

public class ListIndustriesQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<ListIndustriesQuery, IndustryListVm>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<IndustryListVm> Handle(ListIndustriesQuery request, CancellationToken cancellationToken)
    {
        var industries = await _context.Industries
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectToListAsync<IndustrySummaryDto>(_mapper.ConfigurationProvider);

        return new IndustryListVm(industries);
    }
}
