using FluentAssertions;
using NUnit.Framework;
using Porcupine.Application.Common.Services;
using Porcupine.Domain.Common;

namespace Porcupine.Application.UnitTests.Common.Services;

public class EventTypeServiceTests
{
    [Test]
    public void AddEventTypesFromAssemblyLoadsTypesFromProvidedAssembly()
    {
        var svc = new EventTypeService();
        svc.EventTypes.Count().Should().Be(0);

        svc.AddEventTypesFromAssembly<EventTypeServiceTests>();
        svc.EventTypes.Should().HaveCount(4);
    }
}

public record TestEventTypeOne : BaseEvent {}
public record TestEventTypeTwo : BaseEvent {}
public record TestEventTypeOne_One : TestEventTypeOne {}
public record GenericEventType<T> : BaseEvent {}