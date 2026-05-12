using Porcupine.Application.Common.Exceptions;
using Porcupine.Application.LifecycleStages.Queries.ListLifecycleStages;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.FunctionalTests.LifecycleStages.Queries;

using static Testing;

public class ListLifecycleStagesTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireAuthentication() 
    {
        var query = new ListLifecycleStagesQuery { };

        await FluentActions.Invoking(() => SendAsync(query))
            .Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task ShouldReturnValidResult() 
    {
        var userId = await RunAsDefaultUserAsync();

        var query = new ListLifecycleStagesQuery { };
        var result = await SendAsync(query);
        
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.Items.Where(x => x.Name == "One").Should().HaveCount(1);
        result.Items.Where(x => x.Name == "Two").Should().HaveCount(1);
    }

    protected override async Task SeedData()
    {
        await AddAsync<LifecycleStage>(new LifecycleStage("One", 1.0));
        await AddAsync<LifecycleStage>(new LifecycleStage("Two", 2.0));

        await base.SeedData();
    }
}