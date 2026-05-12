using Porcupine.Application.Common.Interfaces;
using Porcupine.Application.Common.Security;
using Porcupine.Domain.Constants;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.Industries.Commands.CreateIndustry;

[Authorize(Roles = Roles.Administrator)]
public record CreateIndustryCommand : IRequest<int>
{
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
}

public class CreateIndustryCommandValidator : AbstractValidator<CreateIndustryCommand>
{
    public CreateIndustryCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class CreateIndustryCommandHandler(IApplicationDbContext context, IUser user) : IRequestHandler<CreateIndustryCommand, int>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IUser _user = user;

    public async Task<int> Handle(CreateIndustryCommand request, CancellationToken cancellationToken)
    {
        var industry = new Industry(request.Name, request.Description);

        _context.Industries.Add(industry);
        await _context.SaveChangesAsync(cancellationToken);

        return industry.Id;
    }
}