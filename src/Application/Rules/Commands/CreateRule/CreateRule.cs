using Porcupine.Application.Common.Interfaces;
using Porcupine.Application.Common.Security;
using Porcupine.Domain.Constants;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.Rules.Commands.CreateRule;

[Authorize(Roles = Roles.Administrator)]
public record CreateRuleCommand : IRequest<int>
{
    public required string Name { get; init; }
    public required Type EventType { get; init; }
    public string? Criteria { get; init; }
}

public class CreateRuleCommandValidator : AbstractValidator<CreateRuleCommand>
{
    public CreateRuleCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.EventType).NotNull();
    }
}

public class CreateRuleCommandHandler(IApplicationDbContext context, IUser user) : IRequestHandler<CreateRuleCommand, int>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IUser _user = user;

    public async Task<int> Handle(CreateRuleCommand request, CancellationToken cancellationToken)
    {
        var rule = Rule.RuleFor(request.EventType, request.Name, request.Criteria);

        _context.Rules.Add(rule);

        await _context.SaveChangesAsync(cancellationToken);

        return rule.Id;
    }
}