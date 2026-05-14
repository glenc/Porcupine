using Porcupine.Domain.Entities;
using Porcupine.Domain.Enums;

namespace Porcupine.Application.Rules.Queries.GetRule;

public record RuleDetailVm(int Id, string Name, TriggerType TriggerType, string TriggerName, string? Criteria)
{
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Rule, RuleDetailVm>();
        }
    }
}