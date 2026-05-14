using Porcupine.Domain.Entities;

namespace Porcupine.Application.Rules.Queries.ListRules;

public record RuleSummaryDto(int Id, string Name, string EventName, string? Criteria) 
{
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Rule, RuleSummaryDto>();
        }
    }
}