using Porcupine.Application.Common.Interfaces;
using Porcupine.Application.Common.Security;
using Porcupine.Domain.Constants;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.Industries.Commands.UpdateIndustry;

[Authorize(Roles = Roles.Administrator)]
public record UpdateIndustryCommand : IRequest<int>
{
    public int Id { get; init; }
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
}

public class UpdateIndustryCommandValidator : AbstractValidator<UpdateIndustryCommand>
{
    public UpdateIndustryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class UpdateIndustryCommandHandler(IApplicationDbContext context, IUser user) : IRequestHandler<UpdateIndustryCommand, int>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IUser _user = user;

    public async Task<int> Handle(UpdateIndustryCommand request, CancellationToken cancellationToken)
    {
        var industry = await _context.Industries.FindAsync([request.Id], cancellationToken)
            ?? throw new NotFoundException("Id", nameof(Industry));
        
        industry.Update(request.Name, request.Description);

        var result = await _context.SaveChangesAsync(cancellationToken);

        return result;
    }
}