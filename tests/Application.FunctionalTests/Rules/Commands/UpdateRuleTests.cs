using Porcupine.Application.Common.Exceptions;
using Porcupine.Application.Rules.Commands.UpdateRule;
using Porcupine.Domain.Entities;
using Porcupine.Domain.Events;

namespace Porcupine.Application.FunctionalTests.Rules.Commands;

using static Testing;

public class UpdateRuleTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new UpdateRuleCommand { };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireAuthentication() 
    {
        var command = new UpdateRuleCommand { Id = 1 };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task ShouldRequireAdministrator() 
    {
        var userId = await RunAsDefaultUserAsync();

        var command = new UpdateRuleCommand { Id = 1 };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task ShouldUpdateRule()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new UpdateRuleCommand { Id = _ruleId, Name = "New name", Criteria = "criteria" };

        var result = await SendAsync(command);

        result.Should().BePositive();

        var rule = await FindAsync<Rule>(_ruleId);
        rule.Should().NotBeNull();
        rule.Name.Should().Be("New name");
        rule.Criteria.Should().Be("criteria");
    }

    int _ruleId;
    protected override async Task SeedData()
    {
        var rule = Rule.DomainEventRuleFor<OrganizationCreatedEvent>("One");
        await AddAsync<Rule>(rule);

        _ruleId = rule.Id;

        await base.SeedData();
    }
}