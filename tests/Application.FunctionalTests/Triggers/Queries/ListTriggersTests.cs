using Porcupine.Application.Common.Exceptions;
using Porcupine.Application.Common.Services;
using Porcupine.Application.Triggers.Queries.ListTriggers;

namespace Porcupine.Application.FunctionalTests.Triggers.Queries;

using static Testing;

public class ListTriggersTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireAuthentication() 
    {
        var query = new ListTriggersQuery { };

        await FluentActions.Invoking(() => SendAsync(query))
            .Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task ShouldReturnValidResult() 
    {
        var triggerService = new TriggerService();
        triggerService.AddTriggersFromAppDomain();
        var triggerCount = triggerService.Triggers.Count;

        var userId = await RunAsDefaultUserAsync();

        var query = new ListTriggersQuery { };
        var result = await SendAsync(query);
        
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(triggerCount);
    }

    protected override async Task SeedData()
    {
        await base.SeedData();
    }
}