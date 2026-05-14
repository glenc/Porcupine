using Porcupine.Application.Common.Exceptions;
using Porcupine.Application.Rules.Queries.GetRule;
using Porcupine.Domain.Entities;
using Porcupine.Domain.Enums;
using Porcupine.Domain.Events;

namespace Porcupine.Application.FunctionalTests.Rules.Queries;

using static Testing;

public class GetRuleTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var userId = await RunAsDefaultUserAsync();

        var query = new GetRuleQuery { };

        await FluentActions.Invoking(() => SendAsync(query))
            .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireAuthentication() 
    {
        var query = new GetRuleQuery { Id = 1 };

        await FluentActions.Invoking(() => SendAsync(query))
            .Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task ShouldReturnValidResult() 
    {
        var userId = await RunAsDefaultUserAsync();

        var query = new GetRuleQuery { Id = _id };
        var result = await SendAsync(query);
        
        result.Should().NotBeNull();
        result.Name.Should().Be("One");
        result.Criteria.Should().Be("Id == 1");
        result.TriggerType.Should().Be(TriggerType.DomainEvent);
        result.TriggerName.Should().Be(typeof(OrganizationCreatedEvent).AssemblyQualifiedName);
    }

    [Test]
    public async Task ShouldThrowIfNotFound()
    {
        var userId = await RunAsDefaultUserAsync();

        var query = new GetRuleQuery { Id = int.MaxValue };

        await FluentActions.Invoking(() => SendAsync(query))
            .Should().ThrowAsync<NotFoundException>();
    }

    private int _id;
    protected override async Task SeedData()
    {
        var rule = Rule.DomainEventRuleFor<OrganizationCreatedEvent>("One", "Id == 1");
        await AddAsync<Rule>(rule);
        _id = rule.Id;

        await base.SeedData();
    }
}