using Porcupine.Domain.Common;

namespace Porcupine.Application.FunctionalTests.EventTypes;

[EventType("Test Event One")]
public record TestEventOne : BaseEvent, IEventTypeNotification;

[EventType("Test Event Two")]
public record TestEventTwo : BaseEvent, IEventTypeNotification;

[EventType("Test Event One_One")]
public record TestEventOne_One : TestEventOne, IEventTypeNotification;