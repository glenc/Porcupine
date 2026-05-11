using Porcupine.Application.Common.Exceptions;
using Porcupine.Application.Industries.Queries.ListIndustries;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.FunctionalTests.Industries.Queries;

using static Testing;

public class ListIndustriesTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireAuthentication() 
    {
        var query = new ListIndustriesQuery { };

        await FluentActions.Invoking(() => SendAsync(query))
            .Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task ShouldReturnValidResult() 
    {
        var userId = await RunAsDefaultUserAsync();

        var query = new ListIndustriesQuery { };
        var result = await SendAsync(query);
        
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.Items.Where(x => x.Name == "One").Should().HaveCount(1);
        result.Items.Where(x => x.Name == "Two").Should().HaveCount(1);
    }

    protected override async Task SeedData()
    {
        await AddAsync<Industry>(new Industry("One", "desc"));
        await AddAsync<Industry>(new Industry("Two", "desc"));

        await base.SeedData();
    }
}