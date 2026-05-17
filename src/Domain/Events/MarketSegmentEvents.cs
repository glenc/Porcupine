using Porcupine.Domain.Triggers;

namespace Porcupine.Domain.Events;


[Trigger("Market Segment Created", "Occurrs when a new market segment is created.")]
public record MarketSegmentCreatedEvent(MarketSegment Entity) 
    : PorcupineEvent<MarketSegment>(Entity), IDomainEventTrigger<MarketSegment> {}

[Trigger("Market Segment Created", "Occurrs when an existing market segment is updated.")]
public record MarketSegmentUpdatedEvent(MarketSegment Entity, IDictionary<string, object?> OriginalState) 
    : PorcupineEntityChangedEvent<MarketSegment>(Entity, OriginalState), IDomainEventTrigger<MarketSegment> {}