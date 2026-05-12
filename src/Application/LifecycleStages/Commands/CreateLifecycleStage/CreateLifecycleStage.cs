using Porcupine.Application.Common.Interfaces;
using Porcupine.Application.Common.Security;
using Porcupine.Domain.Constants;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.LifecycleStages.Commands.CreateLifecycleStage;

[Authorize(Roles = Roles.Administrator)]
public record CreateLifecycleStageCommand : IRequest<int>
{
    public string Name { get; init; } = "";
    public double Order { get; init; }
}

public class CreateLifecycleStageCommandValidator : AbstractValidator<CreateLifecycleStageCommand>
{
    public CreateLifecycleStageCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class CreateLifecycleStageCommandHandler(IApplicationDbContext context, IUser user) : IRequestHandler<CreateLifecycleStageCommand, int>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IUser _user = user;

    public async Task<int> Handle(CreateLifecycleStageCommand request, CancellationToken cancellationToken)
    {
        var entity = new LifecycleStage(request.Name, request.Order);

        _context.LifecycleStages.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}