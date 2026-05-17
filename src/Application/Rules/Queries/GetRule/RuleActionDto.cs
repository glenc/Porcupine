using Porcupine.Domain.ValueObjects;

namespace Porcupine.Application.Rules.Queries.GetRule;

public record RuleActionDto(string CommandTypeName, string CommandTemplate)
{
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<RuleAction, RuleActionDto>();
        }
    }
}