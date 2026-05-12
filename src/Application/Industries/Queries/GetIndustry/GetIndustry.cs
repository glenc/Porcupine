using Porcupine.Application.Common.Interfaces;
using Porcupine.Application.Common.Security;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.Industries.Queries.GetIndustry;

[Authorize]
public record GetIndustryQuery : IRequest<IndustryDetailVm>
{
    public int Id { get; init; }
}

public class GetIndustryQueryValidator : AbstractValidator<GetIndustryQuery>
{
    public GetIndustryQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetIndustryQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetIndustryQuery, IndustryDetailVm>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<IndustryDetailVm> Handle(GetIndustryQuery request, CancellationToken cancellationToken)
    {
        if (await _context.Industries.AnyAsync(x => x.Id == request.Id, cancellationToken) == false)
            throw new NotFoundException("Id", nameof(Industry));
        
        var industry = await _context.Industries.Where(x => x.Id == request.Id)
            .AsNoTracking()
            .FirstAsync(cancellationToken);

        return _mapper.Map<IndustryDetailVm>(industry);
    }
}
