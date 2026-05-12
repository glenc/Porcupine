using Porcupine.Application.Common.Exceptions;
using Porcupine.Application.LifecycleStages.Commands.CreateLifecycleStage;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.FunctionalTests.LifecycleStages.Commands;

using static Testing;

public class CreateLifecycleStageTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new CreateLifecycleStageCommand { };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
        
        command = new CreateLifecycleStageCommand { Order = 2.0 };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireAuthentication() 
    {
        var command = new CreateLifecycleStageCommand { Name = "foo" };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<UnauthorizedAccessException>();
    }
    
    [Test]
    public async Task ShouldRequireAdministratorRole() 
    {
        await ResetState();
        var userId = await RunAsDefaultUserAsync();

        var query = new CreateLifecycleStageCommand { Name = "foo" };

        await FluentActions.Invoking(() => SendAsync(query))
            .Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task ShouldReturnCreateLifecycleStage()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new CreateLifecycleStageCommand { Name = "foo", Order = 2.0 };
        var result = await SendAsync(command);
        
        result.Should().BePositive();

        var lifecyclestage = await FindAsync<LifecycleStage>(result);
        lifecyclestage.Should().NotBeNull();
        lifecyclestage.Name.Should().Be("foo");
        lifecyclestage.Order.Should().Be(2.0);
    }

    protected override async Task SeedData()
    {
        await base.SeedData();
    }
}