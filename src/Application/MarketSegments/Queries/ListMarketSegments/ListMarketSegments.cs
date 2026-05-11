using Porcupine.Application.Common.Interfaces;
using Porcupine.Application.Common.Mappings;
using Porcupine.Application.Common.Security;

namespace Porcupine.Application.MarketSegments.Queries.ListMarketSegments;

[Authorize]
public record ListMarketSegmentsQuery : IRequest<MarketSegmentListVm>
{
}

public class ListMarketSegmentsQueryValidator : AbstractValidator<ListMarketSegmentsQuery>
{
    public ListMarketSegmentsQueryValidator()
    {
    }
}

public class ListMarketSegmentsQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<ListMarketSegmentsQuery, MarketSegmentListVm>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<MarketSegmentListVm> Handle(ListMarketSegmentsQuery request, CancellationToken cancellationToken)
    {
        var segments = await _context.MarketSegments
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectToListAsync<MarketSegmentSummaryDto>(_mapper.ConfigurationProvider);
        
        return new MarketSegmentListVm(segments);
    }
}
