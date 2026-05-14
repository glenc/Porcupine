using Porcupine.Application.Common.Interfaces;
using Porcupine.Application.Common.Mappings;
using Porcupine.Application.Common.Security;

namespace Porcupine.Application.Rules.Queries.ListRules;

[Authorize]
public record ListRulesQuery : IRequest<RuleListVm>
{
}

public class ListRulesQueryValidator : AbstractValidator<ListRulesQuery>
{
    public ListRulesQueryValidator()
    {
    }
}

public class ListRulesQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<ListRulesQuery, RuleListVm>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<RuleListVm> Handle(ListRulesQuery request, CancellationToken cancellationToken)
    {
        var rules = await _context.Rules
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectToListAsync<RuleSummaryDto>(_mapper.ConfigurationProvider);

        return new RuleListVm(rules);
    }
}
