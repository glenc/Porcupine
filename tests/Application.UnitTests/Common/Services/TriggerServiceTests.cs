using FluentAssertions;
using NUnit.Framework;
using Porcupine.Application.Common.Services;
using Porcupine.Domain.Triggers;

namespace Porcupine.Application.UnitTests.Common.Services;

public class TriggerServiceTest
{
    [Test]
    public void AddTriggersFromAssemblyLoadsTriggers()
    {
        var svc = new TriggerService();
        svc.Triggers.Should().HaveCount(0);

        svc.AddTriggersFromAssembly<TriggerServiceTest>();
        svc.Triggers.Should().HaveCount(3);

        svc.Triggers.Where(x => x == typeof(TestEventOne)).Should().HaveCount(1);
        svc.Triggers.Where(x => x == typeof(TestEventTwo)).Should().HaveCount(1);
        svc.Triggers.Where(x => x == typeof(TestEventThree)).Should().HaveCount(1);
    }

    [Test]
    public void AddTriggersFromAppDomainLoadsTriggers()
    {
        var svc = new TriggerService();
        svc.Triggers.Should().HaveCount(0);

        svc.AddTriggersFromAppDomain();
        svc.Triggers.Should().HaveCountGreaterThanOrEqualTo(3);

        foreach (var trigger in svc.Triggers)
        {
            trigger.Should().BeAssignableTo<ITrigger>();
        };
    }

    [Test]
    public void AddTriggerShouldAddTrigger()
    {
        var svc = new TriggerService();
        svc.Triggers.Should().HaveCount(0);

        svc.AddTrigger<TestEventOne>();

        svc.Triggers.Should().HaveCount(1);
        svc.Triggers.Where(x => x == typeof(TestEventOne)).Should().HaveCount(1);
    }

    [Test]
    public void ClearTriggersShouldRemoveAllTriggers()
    {
        var svc = new TriggerService();
        svc.Triggers.Should().HaveCount(0);

        svc.AddTrigger<TestEventOne>();
        
        svc.Triggers.Should().HaveCount(1);

        svc.ClearTriggers();

        svc.Triggers.Should().HaveCount(0);
    }
}

public record TestEventOne : ITrigger {}
public record TestEventTwo : ITrigger {}
public record TestEventThree : TestEventTwo {}