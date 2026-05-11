using Porcupine.Application.Common.Exceptions;
using Porcupine.Application.MarketSegments.Queries.ListMarketSegments;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.FunctionalTests.MarketSegments.Queries;

using static Testing;

public class ListMarketSegmentsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireAuthentication() 
    {
        var query = new ListMarketSegmentsQuery { };

        await FluentActions.Invoking(() => SendAsync(query))
            .Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task ShouldReturnValidResult() 
    {
        var userId = await RunAsDefaultUserAsync();

        var query = new ListMarketSegmentsQuery { };
        var result = await SendAsync(query);
        
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.Items.Where(x => x.Name == "One").Should().HaveCount(1);
        result.Items.Where(x => x.Name == "Two").Should().HaveCount(1);
    }

    protected override async Task SeedData()
    {
        await AddAsync<MarketSegment>(new MarketSegment("One", "desc"));
        await AddAsync<MarketSegment>(new MarketSegment("Two", "desc"));

        await base.SeedData();
    }
}