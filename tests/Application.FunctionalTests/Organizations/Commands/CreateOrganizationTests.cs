using Porcupine.Application.Common.Exceptions;
using Porcupine.Application.Organizations.Commands.CreateOrganization;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.FunctionalTests.Organizations.Commands;

using static Testing;

public class CreateOrganizationTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new CreateOrganizationCommand { };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
        
        command = new CreateOrganizationCommand { Name = "foo" };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
        
        command = new CreateOrganizationCommand { LifecycleStageId = 1 };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireAuthentication() 
    {
        var command = new CreateOrganizationCommand { Name = "foo", LifecycleStageId = 1 };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task ShouldRequireAdministratorRole() 
    {
        await ResetState();
        var userId = await RunAsDefaultUserAsync();

        var command = new CreateOrganizationCommand { Name = "foo", LifecycleStageId = 1 };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task ShouldCreateOrganization()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new CreateOrganizationCommand { Name = "foo", LifecycleStageId = _stageId };
        var result = await SendAsync(command);
        
        result.Should().BePositive();

        var org = await FindAsync<Organization>(result, includes: [(x => x.LifecycleStage), (x => x.Industry), (x => x.MarketSegment)]);
        org.Should().NotBeNull();
        org.LifecycleStage.Should().NotBeNull();
        org.LifecycleStage.Id.Should().Be(_stageId);

        org.Industry.Should().BeNull();
        org.MarketSegment.Should().BeNull();
    }

    [Test]
    public async Task ShouldCreateOrganizationWithIndustry()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new CreateOrganizationCommand { Name = "foo", LifecycleStageId = _stageId, IndustryId = _industryId };
        var result = await SendAsync(command);
        
        result.Should().BePositive();

        var org = await FindAsync<Organization>(result, includes: [(x => x.LifecycleStage), (x => x.Industry), (x => x.MarketSegment)]);
        org.Should().NotBeNull();
        org.LifecycleStage.Should().NotBeNull();
        org.LifecycleStage.Id.Should().Be(_stageId);

        org.Industry.Should().NotBeNull();
        org.Industry.Id.Should().Be(_industryId);

        org.MarketSegment.Should().BeNull();
    }

    [Test]
    public async Task ShouldCreateOrganizationWithMarketSegment()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new CreateOrganizationCommand { Name = "foo", LifecycleStageId = _stageId, MarketSegmentId = _segmentId };
        var result = await SendAsync(command);
        
        result.Should().BePositive();

        var org = await FindAsync<Organization>(result, includes: [(x => x.LifecycleStage), (x => x.Industry), (x => x.MarketSegment)]);
        org.Should().NotBeNull();
        org.LifecycleStage.Should().NotBeNull();
        org.LifecycleStage.Id.Should().Be(_stageId);

        org.MarketSegment.Should().NotBeNull();
        org.MarketSegment.Id.Should().Be(_segmentId);
        
        org.Industry.Should().BeNull();
    }

    [Test]
    public async Task ShouldThrowForInvalidReferenceIds()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new CreateOrganizationCommand { Name = "foo", LifecycleStageId = int.MaxValue };
        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<NotFoundException>();
        
        command = new CreateOrganizationCommand { Name = "foo", LifecycleStageId = _stageId, IndustryId = int.MaxValue };
        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<NotFoundException>();
        
        command = new CreateOrganizationCommand { Name = "foo", LifecycleStageId = _stageId, MarketSegmentId = int.MaxValue };
        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<NotFoundException>();
    }

    private int _stageId;
    private int _industryId;
    private int _segmentId;
    protected override async Task SeedData()
    {
        var stage = new LifecycleStage("Prospect");
        var industry = new Industry("Manufacturing");
        var segment = new MarketSegment("West Coast");

        await AddAsync<LifecycleStage>(stage);
        await AddAsync<Industry>(industry);
        await AddAsync<MarketSegment>(segment);

        _stageId = stage.Id;
        _industryId = industry.Id;
        _segmentId = segment.Id;

        await base.SeedData();
    }
}