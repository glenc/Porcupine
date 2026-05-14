using Porcupine.Application.Common.Interfaces;
using Porcupine.Application.Common.Security;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.Rules.Queries.GetRule;

[Authorize]
public record GetRuleQuery : IRequest<RuleDetailVm>
{
    public int Id { get; init; }
}

public class GetRuleQueryValidator : AbstractValidator<GetRuleQuery>
{
    public GetRuleQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetRuleQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetRuleQuery, RuleDetailVm>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<RuleDetailVm> Handle(GetRuleQuery request, CancellationToken cancellationToken)
    {
        if (await _context.Rules.AnyAsync(x => x.Id == request.Id, cancellationToken) == false)
            throw new NotFoundException("Id", nameof(Rule));
        
        var rule = await _context.Rules.Where(x => x.Id == request.Id)
            .AsNoTracking()
            .FirstAsync(cancellationToken);

        return _mapper.Map<RuleDetailVm>(rule);
    }
}
