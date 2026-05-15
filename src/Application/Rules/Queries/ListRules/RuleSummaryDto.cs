using Porcupine.Domain.Entities;
using Porcupine.Domain.Triggers;

namespace Porcupine.Application.Rules.Queries.ListRules;

public record RuleSummaryDto(int Id, string Name, TriggerType TriggerType, string TriggerName, string? Criteria) 
{
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Rule, RuleSummaryDto>();
        }
    }
}