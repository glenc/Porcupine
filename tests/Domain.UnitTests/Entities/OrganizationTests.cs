using FluentAssertions;
using NUnit.Framework;
using Porcupine.Domain.Common;
using Porcupine.Domain.Entities;
using Porcupine.Domain.Events;

namespace Porcupine.Domain.UnitTests.Entities;

public class OrganizationTests
{
    // seed data
    private readonly LifecycleStage _stage = new("Default");
    private readonly LifecycleStage _stageTwo = new("Stage Two");
    private readonly MarketSegment _segment = new("Segment");
    private readonly Industry _industry = new("Industry");

    // constructor

    [Test]
    public void CanCreateOrganization()
    {
        var entity = new Organization("Acme", _stage);
        entity.Should().NotBeNull();
    }

    [Test]
    public void CreateSetsProperties()
    {
        var entity = new Organization("Acme", _stage);
        entity.Should().NotBeNull();

        entity.Name.Should().Be("Acme");
        entity.LifecycleStage.Should().Be(_stage);
    }

    [Test]
    public void ConstructorGuardsAgainstNullName()
    {
        var act = () => new Organization("", _stage);
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void CreationAddsDomainEvent()
    {
        var entity = new Organization("Acme", _stage);
        entity.DomainEvents.Should().HaveCount(1);
        entity.DomainEvents.First().Should().BeOfType<OrganizationCreatedEvent>();
    }

    // update

    [Test]
    public void UpdateShouldUpdateName()
    {
        var entity = new Organization("Acme", _stage);
        entity.Update("Boom Industries");
        entity.Name.Should().Be("Boom Industries");
    }

    [Test]
    public void UpdateShouldAddDomainEvent()
    {
        var entity = new Organization("Acme", _stage);
        entity.ClearDomainEvents();

        entity.Update("Boom Industries");
        
        entity.DomainEvents.Should().HaveCount(1);
        entity.DomainEvents.First().Should().BeOfType<OrganizationUpdatedEvent>();
    }

    [Test]
    public void UpdateShoulOnlyAddDomainEventWhenValueChanges()
    {
        var entity = new Organization("Acme", _stage);
        entity.ClearDomainEvents();

        entity.Update("Acme");
        
        entity.DomainEvents.Should().HaveCount(0);
    }

    // SetMarketSegment

    [Test]
    public void SetMarketSegmentShouldSetSegment()
    {
        var entity = new Organization("Acme", _stage);
        entity.SetMarketSegment(_segment);
        entity.MarketSegment.Should().Be(_segment);
    }

    [Test]
    public void SetMarketSegmentShouldAddDomainEvent()
    {
        var entity = new Organization("Acme", _stage);
        entity.ClearDomainEvents();

        entity.SetMarketSegment(_segment);
        
        entity.DomainEvents.Should().HaveCount(1);
        entity.DomainEvents.First().Should().BeOfType<OrganizationUpdatedEvent>();
    }

    [Test]
    public void SetMarketSegmentOnlyAddDomainEventWhenValueChanges()
    {
        var entity = new Organization("Acme", _stage);
        entity.SetMarketSegment(_segment);
        entity.ClearDomainEvents();

        entity.SetMarketSegment(_segment);
        
        entity.DomainEvents.Should().HaveCount(0);
    }

    // SetIndustry

    [Test]
    public void SetIndustryShouldSetSegment()
    {
        var entity = new Organization("Acme", _stage);
        entity.SetIndustry(_industry);
        entity.Industry.Should().Be(_industry);
    }

    [Test]
    public void SetIndustryShouldAddDomainEvent()
    {
        var entity = new Organization("Acme", _stage);
        entity.ClearDomainEvents();

        entity.SetIndustry(_industry);
        
        entity.DomainEvents.Should().HaveCount(1);
        entity.DomainEvents.First().Should().BeOfType<OrganizationUpdatedEvent>();
    }

    [Test]
    public void SetIndustryOnlyAddDomainEventWhenValueChanges()
    {
        var entity = new Organization("Acme", _stage);
        entity.SetIndustry(_industry);
        entity.ClearDomainEvents();

        entity.SetIndustry(_industry);
        
        entity.DomainEvents.Should().HaveCount(0);
    }

    // ChangeLifecycleStage
    [Test]
    public void ChangeLifecycleStageShouldSetSegment()
    {
        var entity = new Organization("Acme", _stage);
        entity.ChangeLifecycleStage(_stageTwo);
        entity.LifecycleStage.Should().Be(_stageTwo);
    }

    [Test]
    public void ChangeLifecycleStageShouldAddDomainEvent()
    {
        var entity = new Organization("Acme", _stage);
        entity.ClearDomainEvents();

        entity.ChangeLifecycleStage(_stageTwo);
        
        entity.DomainEvents.Should().HaveCount(1);
        entity.DomainEvents.First().Should().BeOfType<OrganizationLifecycleStageChangedEvent>();
    }

    [Test]
    public void ChangeLifecycleStageOnlyAddDomainEventWhenValueChanges()
    {
        var entity = new Organization("Acme", _stage);
        entity.ChangeLifecycleStage(_stageTwo);
        entity.ClearDomainEvents();

        entity.ChangeLifecycleStage(_stageTwo);
        
        entity.DomainEvents.Should().HaveCount(0);
    }
}