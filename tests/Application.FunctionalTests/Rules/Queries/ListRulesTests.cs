using Porcupine.Application.Common.Exceptions;
using Porcupine.Application.Rules.Queries.ListRules;
using Porcupine.Domain.Common;
using Porcupine.Domain.Entities;
using Porcupine.Domain.Events;

namespace Porcupine.Application.FunctionalTests.Rules.Queries;

using static Testing;

public class ListRulesTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireAuthentication() 
    {
        var query = new ListRulesQuery { };

        await FluentActions.Invoking(() => SendAsync(query))
            .Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task ShouldReturnValidResult() 
    {
        var userId = await RunAsDefaultUserAsync();

        var query = new ListRulesQuery { };
        var result = await SendAsync(query);
        
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.Items.Where(x => x.Name == "One").Should().HaveCount(1);
        result.Items.Where(x => x.Name == "Two").Should().HaveCount(1);

        result.Items.Where(x => x.TriggerName == typeof(OrganizationCreatedEvent).AssemblyQualifiedName).Should().HaveCount(1);
        result.Items.Where(x => x.TriggerName == typeof(OrganizationUpdatedEvent).AssemblyQualifiedName).Should().HaveCount(1);
    }

    protected override async Task SeedData()
    {
        await AddAsync<Rule>(Rule.DomainEventRuleFor<OrganizationCreatedEvent>("One"));
        await AddAsync<Rule>(Rule.DomainEventRuleFor<OrganizationUpdatedEvent>("Two"));

        await base.SeedData();
    }
}