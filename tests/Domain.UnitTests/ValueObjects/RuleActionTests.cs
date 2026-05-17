using FluentAssertions;
using NUnit.Framework;
using Porcupine.Domain.ValueObjects;

namespace Porcupine.Domain.UnitTests.ValueObjects;

public class RuleActionTests
{
    [Test]
    public void RuleActionForReturnsValidAction()
    {
        var action = RuleAction.For<int>("{ Foo: 1 }");
        action.GetCommandType().Should().Be<int>();
        action.CommandTemplate.Should().Be("{ Foo: 1 }");
    }

    [Test]
    public void RuleActionForShouldGuardAgainstEmptyTemplate()
    {
        var act = () => RuleAction.For<int>("");
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void SameActionsShouldBeEqual()
    {
        var action1 = RuleAction.For<int>("{ Foo: 1 }");
        var action2 = RuleAction.For<int>("{ Foo: 1 }");

        action1.Should().Be(action2);

        var action3 = RuleAction.For<int>("{ Foo: 2 }");
        action1.Should().NotBe(action3);
    }
}