using System.Collections.ObjectModel;
using Porcupine.Domain.Entities;
using Porcupine.Domain.Triggers;

namespace Porcupine.Application.Rules.Queries.GetRule;

public record RuleDetailVm(
    int Id, 
    string Name, 
    TriggerType TriggerType, 
    string TriggerName, 
    string? Criteria, 
    IReadOnlyCollection<RuleActionDto> Actions)
{
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Rule, RuleDetailVm>();
        }
    }
}