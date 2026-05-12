using Porcupine.Application.Common.Interfaces;
using Porcupine.Application.Common.Security;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.LifecycleStages.Queries.GetLifecycleStage;

[Authorize]
public record GetLifecycleStageQuery : IRequest<LifecycleStageDetailVm>
{
    public int Id { get; init; }
}

public class GetLifecycleStageQueryValidator : AbstractValidator<GetLifecycleStageQuery>
{
    public GetLifecycleStageQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetLifecycleStageQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetLifecycleStageQuery, LifecycleStageDetailVm>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<LifecycleStageDetailVm> Handle(GetLifecycleStageQuery request, CancellationToken cancellationToken)
    {
        if (await _context.LifecycleStages.AnyAsync(x => x.Id == request.Id, cancellationToken) == false)
            throw new NotFoundException("Id", nameof(LifecycleStage));
        
        var entity = await _context.LifecycleStages.Where(x => x.Id == request.Id)
            .AsNoTracking()
            .FirstAsync(cancellationToken);

        return _mapper.Map<LifecycleStageDetailVm>(entity);
    }
}
