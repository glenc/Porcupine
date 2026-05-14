using Porcupine.Application.Common.Exceptions;
using Porcupine.Application.EventTypes.Queries.ListEventTypes;

namespace Porcupine.Application.FunctionalTests.EventTypes.Queries;

using static Testing;

public class ListEventTypesTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireAuthentication() 
    {
        var query = new ListEventTypesQuery { };

        await FluentActions.Invoking(() => SendAsync(query))
            .Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task ShouldReturnValidResult() 
    {
        var userId = await RunAsDefaultUserAsync();

        var query = new ListEventTypesQuery { };
        var result = await SendAsync(query);
        
        result.Should().NotBeNull();
        result.Items.Should().NotBeNull();
        result.Items.Should().HaveCount(3);

        result.Items.Where(x => x.Name == typeof(TestEventOne).AssemblyQualifiedName).Should().HaveCount(1);
        result.Items.Where(x => x.Name == typeof(TestEventTwo).AssemblyQualifiedName).Should().HaveCount(1);
        result.Items.Where(x => x.Name == typeof(TestEventOne_One).AssemblyQualifiedName).Should().HaveCount(1);
    }

    protected override async Task SeedData()
    {
        await base.SeedData();
    }
}