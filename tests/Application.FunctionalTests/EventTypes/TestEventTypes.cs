using Porcupine.Domain.Common;

namespace Porcupine.Application.FunctionalTests.EventTypes;

public record TestEventOne : BaseEvent;
public record TestEventTwo : BaseEvent;
public record TestEventOne_One : TestEventOne;