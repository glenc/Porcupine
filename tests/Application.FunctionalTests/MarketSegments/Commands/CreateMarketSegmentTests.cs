using Porcupine.Application.Common.Exceptions;
using Porcupine.Application.MarketSegments.Commands.CreateMarketSegment;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.FunctionalTests.MarketSegments.Commands;

using static Testing;

public class CreateMarketSegmentTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new CreateMarketSegmentCommand { };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
        
        command = new CreateMarketSegmentCommand { Description = "asdf" };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireAuthentication() 
    {
        var command = new CreateMarketSegmentCommand { Name = "foo" };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<UnauthorizedAccessException>();
    }
    
    [Test]
    public async Task ShouldRequireAdministratorRole() 
    {
        await ResetState();
        var userId = await RunAsDefaultUserAsync();

        var query = new CreateMarketSegmentCommand { Name = "foo" };

        await FluentActions.Invoking(() => SendAsync(query))
            .Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task ShouldReturnCreateMarketSegment()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new CreateMarketSegmentCommand { Name = "foo", Description = "bar" };
        var result = await SendAsync(command);
        
        result.Should().BePositive();

        var segment = await FindAsync<MarketSegment>(result);
        segment.Should().NotBeNull();
        segment.Name.Should().Be("foo");
        segment.Description.Should().Be("bar");
    }

    protected override async Task SeedData()
    {
        await base.SeedData();
    }
}