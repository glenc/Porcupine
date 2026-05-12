using Porcupine.Application.Common.Exceptions;
using Porcupine.Application.Industries.Queries.GetIndustry;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.FunctionalTests.Industries.Queries;

using static Testing;

public class GetIndustryTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var userId = await RunAsDefaultUserAsync();

        var query = new GetIndustryQuery { };

        await FluentActions.Invoking(() => SendAsync(query))
            .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireAuthentication() 
    {
        var query = new GetIndustryQuery { Id = 1 };

        await FluentActions.Invoking(() => SendAsync(query))
            .Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task ShouldReturnValidResult() 
    {
        var userId = await RunAsDefaultUserAsync();

        var query = new GetIndustryQuery { Id = _id };
        var result = await SendAsync(query);
        
        result.Should().NotBeNull();
        result.Name.Should().Be("One");
        result.Description.Should().Be("Desc");
    }

    [Test]
    public async Task ShouldThrowIfNotFound()
    {
        var userId = await RunAsDefaultUserAsync();

        var query = new GetIndustryQuery { Id = int.MaxValue };

        await FluentActions.Invoking(() => SendAsync(query))
            .Should().ThrowAsync<NotFoundException>();
    }

    private int _id;
    protected override async Task SeedData()
    {
        var entity = new Industry("One", "Desc");
        await AddAsync<Industry>(entity);
        _id = entity.Id;

        await base.SeedData();
    }
}