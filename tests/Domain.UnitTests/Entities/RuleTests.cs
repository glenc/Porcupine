using FluentAssertions;
using NUnit.Framework;
using Porcupine.Domain.Common;
using Porcupine.Domain.Entities;
using Porcupine.Domain.Events;

namespace Porcupine.Domain.UnitTests.Entities;

public class RuleTests
{
    [Test]
    public void CanCreateRule()
    {
        var entity = Rule.RuleFor<object>("name");
        entity.Should().NotBeNull();
    }

    [Test]
    public void CreateRuleSetsProperties()
    {
        var entity = Rule.RuleFor<object>("One", "x == y");
        entity.Should().NotBeNull();
        entity.Name.Should().Be("One");
        entity.EventName.Should().Be("System.Object");
        entity.Criteria.Should().Be("x == y");
    }

    [Test]
    public void EmptyCriteriaSetsNullCriteria()
    {
        var entity = Rule.RuleFor<object>("One");
        entity.Should().NotBeNull();
        entity.Name.Should().Be("One");
        entity.EventName.Should().Be("System.Object");
        entity.Criteria.Should().BeNull();
    }

    [Test]
    public void RuleForGenericTypeWorks()
    {
        var entity = Rule.RuleFor<PorcupineEvent<object>>("One");
        entity.Should().NotBeNull();
        entity.EventName.Should().Be("Porcupine.Domain.Events.PorcupineEvent<System.Object>");
    }

    [Test]
    public void EmptyNameThrowsException()
    {
        var act = () => Rule.RuleFor<object>("", "x == y");
        act.Should().Throw<ArgumentException>();
    }
}