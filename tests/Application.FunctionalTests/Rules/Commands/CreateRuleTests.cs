using Porcupine.Application.Common.Exceptions;
using Porcupine.Application.Rules.Commands.CreateRule;
using Porcupine.Domain.Entities;
using Porcupine.Domain.Triggers;
using Porcupine.Domain.Events;

namespace Porcupine.Application.FunctionalTests.Rules.Commands;

using static Testing;

public class CreateRuleTests : BaseTestFixture
{
    private readonly string _eventName = typeof(OrganizationCreatedEvent).AssemblyQualifiedName 
        ?? throw new Exception("can't resolve type");

    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new CreateRuleCommand { Name = "", TriggerName = "foo", TriggerType = TriggerType.DomainEvent };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
        
        command = new CreateRuleCommand { Name = "foo", TriggerName = "", TriggerType = TriggerType.DomainEvent };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireAuthentication() 
    {
        var command = new CreateRuleCommand { Name = "Test", TriggerName = _eventName, TriggerType = TriggerType.DomainEvent };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task ShouldRequireAdministrator() 
    {
        var userId = await RunAsDefaultUserAsync();

        var command = new CreateRuleCommand { Name = "Test", TriggerName = _eventName, TriggerType = TriggerType.DomainEvent };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task TriggerNameShouldResolveToValidType() 
    {
        var userId = await RunAsAdministratorAsync();

        var command = new CreateRuleCommand { Name = "Test", TriggerName = "NotAType", TriggerType = TriggerType.DomainEvent };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task ShouldCreateANewRule()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new CreateRuleCommand { Name = "Test", TriggerName = _eventName, TriggerType = TriggerType.DomainEvent };
        var result = await SendAsync(command);
        
        result.Should().BePositive();

        var rule = await FindAsync<Rule>(result);
        rule.Should().NotBeNull();
        rule.Name.Should().Be("Test");
        rule.TriggerName.Should().Be(_eventName);
        rule.TriggerType.Should().Be(TriggerType.DomainEvent);
        rule.Criteria.Should().BeNull();
    }

    [Test]
    public async Task ShouldCreateANewRuleWithCriteria()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new CreateRuleCommand { Name = "Test", TriggerName = _eventName, TriggerType = TriggerType.DomainEvent, Criteria = "Id == 1" };
        var result = await SendAsync(command);
        
        result.Should().BePositive();

        var rule = await FindAsync<Rule>(result);
        rule.Should().NotBeNull();
        rule.Name.Should().Be("Test");
        rule.TriggerName.Should().Be(_eventName);
        rule.TriggerType.Should().Be(TriggerType.DomainEvent);
        rule.Criteria.Should().Be("Id == 1");
    }

    protected override async Task SeedData()
    {
        await base.SeedData();
    }
}