using Porcupine.Application.Common.Exceptions;
using Porcupine.Application.Rules.Commands.CreateRule;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.FunctionalTests.Rules.Commands;

using static Testing;

public class CreateRuleTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new CreateRuleCommand { Name = "", EventType = typeof(object) };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireAuthentication() 
    {
        var command = new CreateRuleCommand { Name = "foo", EventType = typeof(object) };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task ShouldRequireAdministrator() 
    {
        var userId = await RunAsDefaultUserAsync();

        var command = new CreateRuleCommand { Name = "foo", EventType = typeof(object) };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task ShouldCreateANewRules()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new CreateRuleCommand { Name = "foo", EventType = typeof(object) };
        var result = await SendAsync(command);
        
        result.Should().BePositive();

        var rule = await FindAsync<Rule>(result);
        rule.Should().NotBeNull();
        rule.Name.Should().Be("foo");
        rule.EventName.Should().Be("System.Object");
        rule.Criteria.Should().BeNull();
    }

    [Test]
    public async Task ShouldCreateANewRulesWithCriteria()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new CreateRuleCommand { Name = "foo", EventType = typeof(object), Criteria = "Id == 1" };
        var result = await SendAsync(command);
        
        result.Should().BePositive();

        var rule = await FindAsync<Rule>(result);
        rule.Should().NotBeNull();
        rule.Name.Should().Be("foo");
        rule.EventName.Should().Be("System.Object");
        rule.Criteria.Should().Be("Id == 1");
    }

    protected override async Task SeedData()
    {
        await base.SeedData();
    }
}