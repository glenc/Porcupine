using Porcupine.Application.Common.Interfaces;
using Porcupine.Application.Common.Security;
using Porcupine.Domain.Constants;
using Porcupine.Domain.Entities;
using Porcupine.Domain.Triggers;

namespace Porcupine.Application.Rules.Commands.CreateRule;

[Authorize(Roles = Roles.Administrator)]
public record CreateRuleCommand() : IRequest<int>
{
    public required string Name { get; init; }
    public required TriggerType TriggerType { get; init; }
    public required string TriggerName { get; init; }
    public string? Criteria { get; init; }
}

public class CreateRuleCommandValidator : AbstractValidator<CreateRuleCommand>
{
    public CreateRuleCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.TriggerName).NotEmpty();
    }
}

public class CreateRuleCommandHandler(IApplicationDbContext context, IUser user) : IRequestHandler<CreateRuleCommand, int>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IUser _user = user;

    public async Task<int> Handle(CreateRuleCommand request, CancellationToken cancellationToken)
    {
        if (request.TriggerType == TriggerType.DomainEvent)
        {
            // try to resolve type
            var eventType = Type.GetType(request.TriggerName) ??
                throw new ArgumentException("TriggerName provided could not be resolved to a valid Type");
            
            var rule = Rule.DomainEventRuleFor(eventType, request.Name, request.Criteria);

            _context.Rules.Add(rule);

            await _context.SaveChangesAsync(cancellationToken);

            return rule.Id;
        }
        else
        {
            throw new ArgumentException($"Rules with triggers of type {request.TriggerType} are not supported.");
        }
    }
}