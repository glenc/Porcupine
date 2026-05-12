using Porcupine.Application.Common.Exceptions;
using Porcupine.Application.LifecycleStages.Queries.GetLifecycleStage;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.FunctionalTests.LifecycleStages.Queries;

using static Testing;

public class GetLifecycleStageTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var userId = await RunAsDefaultUserAsync();

        var query = new GetLifecycleStageQuery { };

        await FluentActions.Invoking(() => SendAsync(query))
            .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireAuthentication() 
    {
        var query = new GetLifecycleStageQuery { Id = 1 };

        await FluentActions.Invoking(() => SendAsync(query))
            .Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task ShouldReturnValidResult() 
    {
        var userId = await RunAsDefaultUserAsync();

        var query = new GetLifecycleStageQuery { Id = _id };
        var result = await SendAsync(query);
        
        result.Should().NotBeNull();
        result.Name.Should().Be("One");
        result.Order.Should().Be(1.0);
    }

    [Test]
    public async Task ShouldThrowIfNotFound()
    {
        var userId = await RunAsDefaultUserAsync();

        var query = new GetLifecycleStageQuery { Id = int.MaxValue };

        await FluentActions.Invoking(() => SendAsync(query))
            .Should().ThrowAsync<NotFoundException>();
    }

    private int _id;
    protected override async Task SeedData()
    {
        var entity = new LifecycleStage("One", 1.0);
        await AddAsync<LifecycleStage>(entity);
        _id = entity.Id;

        await base.SeedData();
    }
}