using FluentAssertions;
using NUnit.Framework;
using Porcupine.Domain.Common;
using Porcupine.Domain.Entities;
using Porcupine.Domain.Triggers;
using Porcupine.Domain.Events;

namespace Porcupine.Domain.UnitTests.Entities;

public class RuleTests
{
    [Test]
    public void DomainEventRuleForCreatesRule()
    {
        var rule = Rule.DomainEventRuleFor<OrganizationCreatedEvent>("test");
        rule.Should().NotBeNull();
        rule.TriggerType.Should().Be(TriggerType.DomainEvent);
        rule.TriggerName.Should().Be(typeof(OrganizationCreatedEvent).AssemblyQualifiedName);
        rule.Name.Should().Be("test");
        rule.Criteria.Should().BeNull();
    }
    [Test]
    public void DomainEventRuleForPopulatesCriteria()
    {
        var rule = Rule.DomainEventRuleFor<OrganizationCreatedEvent>("test", "Id = 1");
        rule.Should().NotBeNull();
        rule.TriggerType.Should().Be(TriggerType.DomainEvent);
        rule.TriggerName.Should().Be(typeof(OrganizationCreatedEvent).AssemblyQualifiedName);
        rule.Name.Should().Be("test");
        rule.Criteria.Should().Be("Id = 1");
    }
    [Test]
    public void DomainEventRuleForWithEmptyNameThrowsException()
    {
        var act = () => Rule.DomainEventRuleFor<OrganizationCreatedEvent>("");
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void DomainEventRuleForAddsDomainEvent()
    {
        var rule = Rule.DomainEventRuleFor<OrganizationCreatedEvent>("test");
        rule.DomainEvents.Should().HaveCount(1);
        rule.DomainEvents.First().Should().BeOfType<RuleCreatedEvent>();
    }

    [Test]
    public void UpdateShouldChangeName()
    {
        var rule = Rule.DomainEventRuleFor<OrganizationCreatedEvent>("test");
        rule.ClearDomainEvents();

        rule.Update("new name");
        rule.Name.Should().Be("new name");

        rule.DomainEvents.Should().HaveCount(1);
        rule.DomainEvents.First().Should().BeOfType<RuleUpdatedEvent>();
    }

    [Test]
    public void UpdateShouldGuardAgainstEmptyName()
    {
        var rule = Rule.DomainEventRuleFor<OrganizationCreatedEvent>("test");
        
        var act = () => rule.Update("");
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void ChangeCriteriaShouldChangeCriteria()
    {
        var rule = Rule.DomainEventRuleFor<OrganizationCreatedEvent>("test", "Id = 1");
        rule.Criteria.Should().Be("Id = 1");
        rule.ClearDomainEvents();

        rule.ChangeCriteria("Id = 2");
        rule.Criteria.Should().Be("Id = 2");

        rule.DomainEvents.Should().HaveCount(1);
        rule.DomainEvents.First().Should().BeOfType<RuleUpdatedEvent>();
    }
}