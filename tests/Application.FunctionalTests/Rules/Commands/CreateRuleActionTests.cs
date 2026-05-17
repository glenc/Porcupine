using Microsoft.AspNetCore.Http.HttpResults;
using Porcupine.Application.Common.Exceptions;
using Porcupine.Application.Organizations.Commands.CreateOrganization;
using Porcupine.Application.Rules.Commands.CreateRuleAction;
using Porcupine.Domain.Entities;
using Porcupine.Domain.Events;

namespace Porcupine.Application.FunctionalTests.Rules.Commands;

using static Testing;

public class CreateRuleActionTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new CreateRuleActionCommand { };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
        
        command = new CreateRuleActionCommand { RuleId = 1 };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
        
        command = new CreateRuleActionCommand
        {
            RuleId = 1,
            CommandTypeName = typeof(CreateOrganizationCommand).AssemblyQualifiedName ?? "",
            CommandTemplate = ""
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireAuthentication() 
    {
        var command = new CreateRuleActionCommand
        {
            RuleId = 1,
            CommandTypeName = typeof(CreateOrganizationCommand).AssemblyQualifiedName ?? "",
            CommandTemplate = "{}"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task ShouldRequireAdministrator() 
    {
        var userId = await RunAsDefaultUserAsync();

        var command = new CreateRuleActionCommand
        {
            RuleId = 1,
            CommandTypeName = typeof(CreateOrganizationCommand).AssemblyQualifiedName ?? "",
            CommandTemplate = "{}"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task ShouldAddActionToRule()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new CreateRuleActionCommand
        {
            RuleId = _ruleId,
            CommandTypeName = typeof(CreateOrganizationCommand).AssemblyQualifiedName ?? "",
            CommandTemplate = "{}"
        };

        var result = await SendAsync(command);
        
        result.Should().BePositive();

        var rule = await FindAsync<Rule>(_ruleId);
        rule.Should().NotBeNull();
        rule.Actions.Should().HaveCount(1);
    }

    [Test]
    public async Task ShouldThrowExceptionForInvalidRule() 
    {
        var userId = await RunAsAdministratorAsync();

        var command = new CreateRuleActionCommand
        {
            RuleId = int.MaxValue,
            CommandTypeName = typeof(CreateOrganizationCommand).AssemblyQualifiedName ?? "",
            CommandTemplate = "{}"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldThrowExceptionForInvalidCommandType() 
    {
        var userId = await RunAsAdministratorAsync();

        var command = new CreateRuleActionCommand
        {
            RuleId = 1,
            CommandTypeName = "some.crap",
            CommandTemplate = "{}"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task ShouldThrowExceptionForInvalidTypeNotImplementingIRequest() 
    {
        var userId = await RunAsAdministratorAsync();

        var command = new CreateRuleActionCommand
        {
            RuleId = 1,
            CommandTypeName = typeof(Rule).AssemblyQualifiedName ?? "",
            CommandTemplate = "{}"
        };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ArgumentException>();
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