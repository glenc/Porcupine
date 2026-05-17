using Porcupine.Domain.Triggers;

namespace Porcupine.Domain.Events;

[Trigger("Organization Created", "Occurrs when a new organization is created.")]
public record OrganizationCreatedEvent(Organization Entity) 
    : PorcupineEvent<Organization>(Entity), IDomainEventTrigger<Organization> {}

[Trigger("Organization Updated", "Occurrs when an existing organization is updated.")]
public record OrganizationUpdatedEvent(Organization Entity, IDictionary<string, object?> OriginalState) 
    : PorcupineEntityChangedEvent<Organization>(Entity, OriginalState), IDomainEventTrigger<Organization> {}

[Trigger("Organization Lifecycle Stage Changed", "Occurrs when the lifecycle stage is changed for an organization.")]
public record OrganizationLifecycleStageChangedEvent(Organization Entity, IDictionary<string, object?> OriginalState) 
    : PorcupineEntityChangedEvent<Organization>(Entity, OriginalState), IDomainEventTrigger<Organization> {}