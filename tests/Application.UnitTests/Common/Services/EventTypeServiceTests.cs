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

        var evt1 = svc.EventTypes.Where(x => x.Type == typeof(TestEventTypeOne)).First();
        evt1.Should().NotBeNull();

        evt1.DisplayName.Should().Be("Test Event One");
        evt1.Description.Should().BeNullOrEmpty();

        var evt2 = svc.EventTypes.Where(x => x.Type == typeof(GenericEventType<>)).First();
        evt2.Should().NotBeNull();

        evt2.DisplayName.Should().Be("Generic");
        evt2.Description.Should().Be("Hard to pin down");
    }

    [Test]
    public void IsTypeRegisteredShouldCheckForEventTypePresence()
    {
        var svc = new EventTypeService();
        svc.AddEventTypesFromAssembly<EventTypeServiceTests>();

        svc.IsTypeRegistered(typeof(TestEventTypeOne)).Should().BeTrue();
        svc.IsTypeRegistered(typeof(object)).Should().BeFalse();
    }
}

[EventType("Test Event One")]
public record TestEventTypeOne : BaseEvent {}

[EventType("Test Event Two")]
public record TestEventTypeTwo : BaseEvent {}

public record TestEventTypeOne_One : TestEventTypeOne {}

[EventType("Generic", "Hard to pin down")]
public record GenericEventType<T> : BaseEvent {}