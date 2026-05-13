using Porcupine.Application.Common.Exceptions;
using Porcupine.Application.Organizations.Queries.ListOrganizations;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.FunctionalTests.Organizations.Queries;

using static Testing;

public class ListOrganizationsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireAuthentication() 
    {
        var query = new ListOrganizationsQuery { };

        await FluentActions.Invoking(() => SendAsync(query))
            .Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task ShouldReturnValidResult() 
    {
        var userId = await RunAsDefaultUserAsync();

        var query = new ListOrganizationsQuery { };
        var result = await SendAsync(query);
        
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(3);
        result.Items.Where(x => x.Name == "One").Should().HaveCount(1);
        result.Items.Where(x => x.Name == "Two").Should().HaveCount(1);
        result.Items.Where(x => x.Name == "Three").Should().HaveCount(1);
    }

    protected override async Task SeedData()
    {
        await ExecuteBatchAsync(async context => {
            var stage = new LifecycleStage("Prospect");

            context.Add(stage);

            await context.SaveChangesAsync();

            context.Add(new Organization("One", stage));
            context.Add(new Organization("Two", stage));
            context.Add(new Organization("Three", stage));

            await context.SaveChangesAsync();
        });

        await base.SeedData();
    }
}