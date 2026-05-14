using Porcupine.Application.Common.Interfaces;
using Porcupine.Application.Common.Security;
using Porcupine.Domain.Constants;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.Rules.Commands.UpdateRule;

[Authorize(Roles = Roles.Administrator)]
public record UpdateRuleCommand : IRequest<int>
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Criteria { get; init; }
}

public class UpdateRuleCommandValidator : AbstractValidator<UpdateRuleCommand>
{
    public UpdateRuleCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class UpdateRuleCommandHandler(IApplicationDbContext context, IUser user) : IRequestHandler<UpdateRuleCommand, int>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IUser _user = user;

    public async Task<int> Handle(UpdateRuleCommand request, CancellationToken cancellationToken)
    {
        var rule = await _context.Rules.FindAsync([request.Id], cancellationToken)
            ?? throw new NotFoundException("Id", nameof(Rule));
        
        if (request.Name != null)
        {
            rule.Update(request.Name);
        }

        if (request.Criteria != null)
        {
            rule.ChangeCriteria(request.Criteria);
        }

        var result = await _context.SaveChangesAsync(cancellationToken);

        return result;
    }
}