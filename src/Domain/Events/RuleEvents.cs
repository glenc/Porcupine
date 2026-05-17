using Porcupine.Domain.ValueObjects;

namespace Porcupine.Domain.Events;

public record RuleCreatedEvent(Rule Entity) 
    : PorcupineEvent<Rule>(Entity) {}

public record RuleUpdatedEvent(Rule Entity, IDictionary<string, object?> OriginalState) 
    : PorcupineEntityChangedEvent<Rule>(Entity, OriginalState) {}

public record ActionAddedToRuleEvent(Rule Entity, RuleAction Action)
    : PorcupineEvent<Rule>(Entity) {}

public record ActionRemovedFromRuleEvent(Rule Entity, RuleAction Action)
    : PorcupineEvent<Rule>(Entity) {}

public record AllActionsRemovedFromRuleEvent(Rule Entity, IEnumerable<RuleAction> ActionsRemoved)
    : PorcupineEvent<Rule>(Entity) {}