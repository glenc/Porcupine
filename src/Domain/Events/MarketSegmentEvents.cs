namespace Porcupine.Domain.Events;

public record MarketSegmentCreatedEvent(MarketSegment Entity) : PorcupineEvent<MarketSegment>(Entity) {}
public record MarketSegmentUpdatedEvent(MarketSegment Entity, IDictionary<string, object> OriginalState) : PorcupineEvent<MarketSegment>(Entity) {}