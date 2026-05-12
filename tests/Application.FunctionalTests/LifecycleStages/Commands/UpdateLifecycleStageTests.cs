using Porcupine.Application.Common.Exceptions;
using Porcupine.Application.LifecycleStages.Commands.UpdateLifecycleStage;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.FunctionalTests.LifecycleStages.Commands;

using static Testing;

public class UpdateLifecycleStageTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new UpdateLifecycleStageCommand { };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireAuthentication() 
    {
        var command = new UpdateLifecycleStageCommand { Id = 1 };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<UnauthorizedAccessException>();
    }
    
    [Test]
    public async Task ShouldRequireAdministratorRole() 
    {
        await ResetState();
        var userId = await RunAsDefaultUserAsync();

        var query = new UpdateLifecycleStageCommand { Id = 1 };

        await FluentActions.Invoking(() => SendAsync(query))
            .Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task ShouldUpdateLifecycleStage()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new UpdateLifecycleStageCommand { Id = _id, Name = "New Name", Order = 2.0 };
        var result = await SendAsync(command);
        
        result.Should().BePositive();

        var lifecyclestage = await FindAsync<LifecycleStage>(_id);
        lifecyclestage.Should().NotBeNull();
        lifecyclestage.Name.Should().Be("New Name");
        lifecyclestage.Order.Should().Be(2.0);
    }

    [Test]
    public async Task ShouldThrowIfNotFound()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new UpdateLifecycleStageCommand { Id = int.MaxValue, Name = "New Name", Order = 2.0 };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<NotFoundException>();
    }

    private int _id;
    protected override async Task SeedData()
    {
        var entity = new LifecycleStage("foo", 1.0);
        await AddAsync<LifecycleStage>(entity);
        _id = entity.Id;

        await base.SeedData();
    }
}