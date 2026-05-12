namespace Porcupine.Domain.Events;

public record OrganizationCreatedEvent(Organization Entity) : PorcupineEvent<Organization>(Entity) {}
public record OrganizationUpdatedEvent(Organization Entity, IDictionary<string, object?> OriginalState) : PorcupineEntityChangedEvent<Organization>(Entity, OriginalState) {}
public record OrganizationLifecycleStageChangedEvent(Organization Entity, IDictionary<string, object?> OriginalState) : PorcupineEntityChangedEvent<Organization>(Entity, OriginalState) {}