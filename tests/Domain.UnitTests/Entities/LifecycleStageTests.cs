using FluentAssertions;
using NUnit.Framework;
using Porcupine.Domain.Common;
using Porcupine.Domain.Entities;
using Porcupine.Domain.Events;

namespace Porcupine.Domain.UnitTests.Entities;

public class LifecycleStageTests
{
    [Test]
    public void CanCreateLifecycleStage()
    {
        var entity = new LifecycleStage("Prospect");
        entity.Should().NotBeNull();
    }

    [Test]
    public void CreateWithNameSetsNameAndDefaultOrder()
    {
        var entity = new LifecycleStage("Prospect");
        entity.Should().NotBeNull();
        entity.Name.Should().Be("Prospect");
        entity.Order.Should().Be(1.0);
    }

    [Test]
    public void CreateWithNameAndOrder()
    {
        var entity = new LifecycleStage("Prospect", 2.0);
        entity.Should().NotBeNull();
        entity.Name.Should().Be("Prospect");
        entity.Order.Should().Be(2.0);
    }

    [Test]
    public void CreateWithEmptyNameThrowsException()
    {
        var action = () => new LifecycleStage("");
        action.Should().Throw<ArgumentException>();

        action = () => new LifecycleStage("", 2.0);
        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void UpdateShouldUpdateNameAndOrder()
    {
        var entity = new LifecycleStage("name", 1.0);
        entity.Name.Should().Be("name");
        entity.Order.Should().Be(1.0);

        entity.Update("new name", 2.0);
        entity.Name.Should().Be("new name");
        entity.Order.Should().Be(2.0);
    }

    [Test]
    public void UpdateShouldUpdateGuardAgainstEmptyName()
    {
        var entity = new LifecycleStage("name", 1.0);
        entity.Name.Should().Be("name");
        entity.Order.Should().Be(1.0);

        var act = () => entity.Update("", 2.0);
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void UpdateShouldRaiseDomainEvent()
    {
        var entity = new LifecycleStage("name", 1.0);
        entity.Name.Should().Be("name");
        entity.Order.Should().Be(1.0);

        entity.ClearDomainEvents();

        entity.Update("new", 2.0);
        entity.DomainEvents.Should().HaveCount(1);
        entity.DomainEvents.First().Should().BeOfType<LifecycleStageUpdatedEvent>();
    }

    [Test]
    public void UpdateDomainEventShouldContainChangedPropertiesOnly()
    {
        var entity = new LifecycleStage("name", 1.0);
        entity.Name.Should().Be("name");
        entity.Order.Should().Be(1.0);

        entity.ClearDomainEvents();

        entity.Update("new", 2.0);
        entity.DomainEvents.Should().HaveCount(1);

        var evt = entity.DomainEvents.First() as LifecycleStageUpdatedEvent;
        evt.Should().NotBeNull();
        evt.OriginalState["Name"].Should().Be("name");
        evt.OriginalState["Order"].Should().Be(1.0);

        entity.ClearDomainEvents();

        entity.Update("new", 3.0);
        entity.DomainEvents.Should().HaveCount(1);

        evt = entity.DomainEvents.First() as LifecycleStageUpdatedEvent;
        evt.Should().NotBeNull();
        evt.OriginalState.Should().NotContainKey("Name");
        evt.OriginalState["Order"].Should().Be(2.0);

        entity.ClearDomainEvents();

        entity.Update("totally new", 3.0);
        entity.DomainEvents.Should().HaveCount(1);

        evt = entity.DomainEvents.First() as LifecycleStageUpdatedEvent;
        evt.Should().NotBeNull();
        evt.OriginalState["Name"].Should().Be("new");
        evt.OriginalState.Should().NotContainKey("Order");
    }
}