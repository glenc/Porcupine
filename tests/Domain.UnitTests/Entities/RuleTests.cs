using FluentAssertions;
using NUnit.Framework;
using Porcupine.Domain.Common;
using Porcupine.Domain.Entities;
using Porcupine.Domain.Enums;
using Porcupine.Domain.Events;

namespace Porcupine.Domain.UnitTests.Entities;

public class RuleTests
{
    [Test]
    public void CanCreateDomainEventRule()
    {
        var rule = Rule.DomainEventRuleFor<OrganizationCreatedEvent>("test");
        rule.Should().NotBeNull();
        rule.TriggerType.Should().Be(TriggerType.DomainEvent);
        rule.TriggerName.Should().Be(typeof(OrganizationCreatedEvent).AssemblyQualifiedName);
        rule.Name.Should().Be("test");
        rule.Criteria.Should().BeNull();
    }

    [Test]
    public void CreateDomainEventRuleGuardsAgainstEmptyName()
    {
        var act = () => Rule.DomainEventRuleFor<OrganizationCreatedEvent>("");
        act.Should().Throw<ArgumentException>();
    }
}