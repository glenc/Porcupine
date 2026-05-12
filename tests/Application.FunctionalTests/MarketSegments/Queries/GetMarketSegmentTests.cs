using Porcupine.Application.Common.Exceptions;
using Porcupine.Application.MarketSegments.Queries.GetMarketSegment;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.FunctionalTests.MarketSegments.Queries;

using static Testing;

public class GetMarketSegmentTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var userId = await RunAsDefaultUserAsync();

        var query = new GetMarketSegmentQuery { };

        await FluentActions.Invoking(() => SendAsync(query))
            .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireAuthentication() 
    {
        var query = new GetMarketSegmentQuery { Id = 1 };

        await FluentActions.Invoking(() => SendAsync(query))
            .Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task ShouldReturnValidResult() 
    {
        var userId = await RunAsDefaultUserAsync();

        var query = new GetMarketSegmentQuery { Id = _id };
        var result = await SendAsync(query);
        
        result.Should().NotBeNull();
        result.Name.Should().Be("One");
        result.Description.Should().Be("Desc");
    }

    [Test]
    public async Task ShouldThrowIfNotFound()
    {
        var userId = await RunAsDefaultUserAsync();

        var query = new GetMarketSegmentQuery { Id = int.MaxValue };

        await FluentActions.Invoking(() => SendAsync(query))
            .Should().ThrowAsync<NotFoundException>();
    }

    private int _id;
    protected override async Task SeedData()
    {
        var entity = new MarketSegment("One", "Desc");
        await AddAsync<MarketSegment>(entity);
        _id = entity.Id;

        await base.SeedData();
    }
}