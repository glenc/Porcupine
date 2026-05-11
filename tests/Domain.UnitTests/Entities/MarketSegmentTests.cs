using FluentAssertions;
using NUnit.Framework;
using Porcupine.Domain.Common;
using Porcupine.Domain.Entities;
using Porcupine.Domain.Events;

namespace Porcupine.Domain.UnitTests.Entities;

public class MarketSegmentTests
{
    [Test]
    public void CanCreateMarketSegment()
    {
        var entity = new MarketSegment("segment");
        entity.Should().NotBeNull();
    }

    [Test]
    public void CreateWithNameSetsNameAndBlankDescription()
    {
        var entity = new MarketSegment("segment");
        entity.Should().NotBeNull();
        entity.Name.Should().Be("segment");
        entity.Description.Should().BeNullOrEmpty();
    }

    [Test]
    public void CreateSetsNameAndDescription()
    {
        var entity = new MarketSegment("segment", "description");
        entity.Should().NotBeNull();
        entity.Name.Should().Be("segment");
        entity.Description.Should().Be("description");
    }

    [Test]
    public void CreateWithEmptyNameThrowsException()
    {
        var action = () => new MarketSegment("");
        action.Should().Throw<ArgumentException>();

        action = () => new MarketSegment("", "asdf");
        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void CreateAddsDomainEvent()
    {
        var entity = new MarketSegment("asdf");
        entity.DomainEvents.Should().HaveCount(1);
        entity.DomainEvents.First().Should().BeOfType<MarketSegmentCreatedEvent>();
    }

    [Test]
    public void UpdateShouldUpdateNameAndDescription()
    {
        var entity = new MarketSegment("name", "description");
        entity.Name.Should().Be("name");
        entity.Description.Should().Be("description");

        entity.Update("new name", "new description");
        entity.Name.Should().Be("new name");
        entity.Description.Should().Be("new description");
    }

    [Test]
    public void UpdateShouldUpdateGuardAgainstEmptyName()
    {
        var entity = new MarketSegment("name", "description");
        entity.Name.Should().Be("name");
        entity.Description.Should().Be("description");

        var act = () => entity.Update("", "new description");
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void UpdateShouldRaiseDomainEvent()
    {
        var entity = new MarketSegment("name", "description");
        entity.Name.Should().Be("name");
        entity.Description.Should().Be("description");

        entity.ClearDomainEvents();

        entity.Update("new", "new");
        entity.DomainEvents.Should().HaveCount(1);
        entity.DomainEvents.First().Should().BeOfType<MarketSegmentUpdatedEvent>();
    }

    [Test]
    public void UpdateDomainEventShouldContainChangedPropertiesOnly()
    {
        var entity = new MarketSegment("name", "description");
        entity.Name.Should().Be("name");
        entity.Description.Should().Be("description");

        entity.ClearDomainEvents();

        entity.Update("new", "new");
        entity.DomainEvents.Should().HaveCount(1);

        var evt = entity.DomainEvents.First() as MarketSegmentUpdatedEvent;
        evt.Should().NotBeNull();
        evt.OriginalState["Name"].Should().Be("name");
        evt.OriginalState["Description"].Should().Be("description");

        entity.ClearDomainEvents();

        entity.Update("new", "totally new");
        entity.DomainEvents.Should().HaveCount(1);

        evt = entity.DomainEvents.First() as MarketSegmentUpdatedEvent;
        evt.Should().NotBeNull();
        evt.OriginalState.Should().NotContainKey("Name");
        evt.OriginalState["Description"].Should().Be("new");

        entity.ClearDomainEvents();

        entity.Update("totally new", "totally new");
        entity.DomainEvents.Should().HaveCount(1);

        evt = entity.DomainEvents.First() as MarketSegmentUpdatedEvent;
        evt.Should().NotBeNull();
        evt.OriginalState["Name"].Should().Be("new");
        evt.OriginalState.Should().NotContainKey("Description");
    }
}