using Porcupine.Application.Common.Exceptions;
using Porcupine.Application.MarketSegments.Commands.UpdateMarketSegment;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.FunctionalTests.MarketSegments.Commands;

using static Testing;

public class UpdateMarketSegmentTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new UpdateMarketSegmentCommand { };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireAuthentication() 
    {
        var command = new UpdateMarketSegmentCommand { Id = 1 };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<UnauthorizedAccessException>();
    }
    
    [Test]
    public async Task ShouldRequireAdministratorRole() 
    {
        await ResetState();
        var userId = await RunAsDefaultUserAsync();

        var query = new UpdateMarketSegmentCommand { Id = 1 };

        await FluentActions.Invoking(() => SendAsync(query))
            .Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task ShouldUpdateMarketSegment()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new UpdateMarketSegmentCommand { Id = _id, Name = "New Name", Description = "New Description" };
        var result = await SendAsync(command);
        
        result.Should().BePositive();

        var segment = await FindAsync<MarketSegment>(_id);
        segment.Should().NotBeNull();
        segment.Name.Should().Be("New Name");
        segment.Description.Should().Be("New Description");
    }

    [Test]
    public async Task ShouldThrowIfNotFound()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new UpdateMarketSegmentCommand { Id = int.MaxValue, Name = "New Name", Description = "New Description" };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<NotFoundException>();
    }

    private int _id;
    protected override async Task SeedData()
    {
        var entity = new MarketSegment("foo", "bar");
        await AddAsync<MarketSegment>(entity);
        _id = entity.Id;

        await base.SeedData();
    }
}