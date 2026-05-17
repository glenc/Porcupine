using Porcupine.Application.Common.Interfaces;
using Porcupine.Application.Common.Security;
using Porcupine.Domain.Constants;
using Porcupine.Domain.Entities;
using Porcupine.Domain.ValueObjects;

namespace Porcupine.Application.Rules.Commands.CreateRuleAction;

[Authorize(Roles = Roles.Administrator)]
public record CreateRuleActionCommand : IRequest<int>
{
    public int RuleId { get; init; }
    public string CommandTypeName { get; init; } = null!;
    public string CommandTemplate { get; init; } = null!;
}

public class CreateRuleActionCommandValidator : AbstractValidator<CreateRuleActionCommand>
{
    public CreateRuleActionCommandValidator()
    {
        RuleFor(x => x.RuleId)
            .NotEmpty();

        RuleFor(x => x.CommandTypeName)
            .NotEmpty();
        
        RuleFor(x => x.CommandTemplate)
            .NotEmpty();
    }
}

public class CreateRuleActionCommandHandler(IApplicationDbContext context, IUser user) : IRequestHandler<CreateRuleActionCommand, int>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IUser _user = user;

    public async Task<int> Handle(CreateRuleActionCommand request, CancellationToken cancellationToken)
    {
        var rule = await _context.Rules.FindAsync([request.RuleId], cancellationToken)
            ?? throw new NotFoundException("Id", nameof(Rule));
        
        var type = Type.GetType(request.CommandTypeName) ??
                throw new ArgumentException("CommandType provided could not be resolved to a valid Type");
        
        if (IsRequest(type) == false)
            throw new ArgumentException("CommandType provided is not a valid IRequest");
        
        rule.AddAction(new RuleAction(type, request.CommandTemplate));

        var result = await _context.SaveChangesAsync(cancellationToken);

        return result;
    }

    private static bool IsRequest(Type type)
    {
        return typeof(IRequest).IsAssignableFrom(type)
            ||
            type.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IRequest<>));
    }
}