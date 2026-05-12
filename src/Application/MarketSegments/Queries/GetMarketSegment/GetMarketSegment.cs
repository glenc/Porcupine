using Porcupine.Application.Common.Interfaces;
using Porcupine.Application.Common.Security;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.MarketSegments.Queries.GetMarketSegment;

[Authorize]
public record GetMarketSegmentQuery : IRequest<MarketSegmentDetailVm>
{
    public int Id { get; init; }
}

public class GetMarketSegmentQueryValidator : AbstractValidator<GetMarketSegmentQuery>
{
    public GetMarketSegmentQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetMarketSegmentQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetMarketSegmentQuery, MarketSegmentDetailVm>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<MarketSegmentDetailVm> Handle(GetMarketSegmentQuery request, CancellationToken cancellationToken)
    {
        if (await _context.MarketSegments.AnyAsync(x => x.Id == request.Id, cancellationToken) == false)
            throw new NotFoundException("Id", nameof(MarketSegment));
        
        var industry = await _context.MarketSegments.Where(x => x.Id == request.Id)
            .AsNoTracking()
            .FirstAsync(cancellationToken);

        return _mapper.Map<MarketSegmentDetailVm>(industry);
    }
}
