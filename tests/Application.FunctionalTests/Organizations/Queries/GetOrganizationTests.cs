using Porcupine.Application.Common.Exceptions;
using Porcupine.Application.Organizations.Queries.GetOrganization;
using Porcupine.Domain.Entities;
using Porcupine.Infrastructure.Data;

namespace Porcupine.Application.FunctionalTests.Organizations.Queries;

using static Testing;

public class GetOrganizationTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var userId = await RunAsDefaultUserAsync();

        var query = new GetOrganizationQuery { };

        await FluentActions.Invoking(() => SendAsync(query))
            .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireAuthentication() 
    {
        var query = new GetOrganizationQuery { Id = 1 };

        await FluentActions.Invoking(() => SendAsync(query))
            .Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task ShouldReturnValidResult() 
    {
        var userId = await RunAsDefaultUserAsync();

        var query = new GetOrganizationQuery { Id = _orgId };
        var result = await SendAsync(query);
        
        result.Should().NotBeNull();
        
        result.Name.Should().Be("Test");
        result.LifecycleStage.Id.Should().Be(_stageId);
        
        result.Industry.Should().NotBeNull();
        result.Industry.Id.Should().Be(_industryId);

        result.MarketSegment.Should().NotBeNull();
        result.MarketSegment.Id.Should().Be(_segmentId);
    }

    [Test]
    public async Task ShouldThrowForInvalidId() 
    {
        var userId = await RunAsDefaultUserAsync();

        var query = new GetOrganizationQuery { Id = int.MaxValue };

        await FluentActions.Invoking(() => SendAsync(query))
            .Should().ThrowAsync<NotFoundException>();
    }

    private int _stageId;
    private int _industryId;
    private int _segmentId;
    private int _orgId;
    protected override async Task SeedData()
    {
        await ExecuteBatchAsync(async context => {
            var stage = new LifecycleStage("Prospect");
            var industry = new Industry("Manufacturing");
            var segment = new MarketSegment("West Coast");

            context.Add(stage);
            context.Add(industry);
            context.Add(segment);

            await context.SaveChangesAsync();

            _stageId = stage.Id;
            _industryId = industry.Id;
            _segmentId = segment.Id;

            var org = new Organization("Test", stage);
            org.SetIndustry(industry);
            org.SetMarketSegment(segment);

            context.Add(org);

            await context.SaveChangesAsync();

            _orgId = org.Id;
        });
        
        await base.SeedData();
    }
}