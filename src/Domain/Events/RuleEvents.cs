namespace Porcupine.Domain.Events;

public record RuleCreatedEvent(Rule Entity) : PorcupineEvent<Rule>(Entity) {}
public record RuleUpdatedEvent(Rule Entity, IDictionary<string, object?> OriginalState) : PorcupineEntityChangedEvent<Rule>(Entity, OriginalState) {}