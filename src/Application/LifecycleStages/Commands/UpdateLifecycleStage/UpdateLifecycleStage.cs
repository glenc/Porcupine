using Porcupine.Application.Common.Interfaces;
using Porcupine.Application.Common.Security;
using Porcupine.Domain.Constants;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.LifecycleStages.Commands.UpdateLifecycleStage;

[Authorize(Roles = Roles.Administrator)]
public record UpdateLifecycleStageCommand : IRequest<int>
{
    public int Id { get; init; }
    public string Name { get; init; } = "";
    public double Order { get; init; }
}

public class UpdateLifecycleStageCommandValidator : AbstractValidator<UpdateLifecycleStageCommand>
{
    public UpdateLifecycleStageCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class UpdateLifecycleStageCommandHandler(IApplicationDbContext context, IUser user) : IRequestHandler<UpdateLifecycleStageCommand, int>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IUser _user = user;

    public async Task<int> Handle(UpdateLifecycleStageCommand request, CancellationToken cancellationToken)
    {
        var lifecycleStage = await _context.LifecycleStages.FindAsync([request.Id], cancellationToken)
            ?? throw new NotFoundException("Id", nameof(LifecycleStage));
        
        lifecycleStage.Update(request.Name, request.Order);

        var result = await _context.SaveChangesAsync(cancellationToken);

        return result;
    }
}