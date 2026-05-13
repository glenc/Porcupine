using Porcupine.Application.Common.Exceptions;
using Porcupine.Application.Organizations.Commands.UpdateOrganization;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.FunctionalTests.Organizations.Commands;

using static Testing;

public class UpdateOrganizationTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new UpdateOrganizationCommand { };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireAuthentication() 
    {
        var command = new UpdateOrganizationCommand { Id = 1 };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task ShouldRequireAdministratorRole() 
    {
        await ResetState();
        var userId = await RunAsDefaultUserAsync();

        var query = new UpdateOrganizationCommand { Id = 1 };

        await FluentActions.Invoking(() => SendAsync(query))
            .Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task ShouldThrowIfNotFound()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new UpdateOrganizationCommand { Id = int.MaxValue };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldUpdateNameIfProvided()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new UpdateOrganizationCommand { Id = _orgId, Name = "New Name" };
        var result = await SendAsync(command);
        
        result.Should().BePositive();

        var org = await FindAsync<Organization>(result);
        org.Should().NotBeNull();
        org.Name.Should().Be("New Name");
    }

    [Test]
    public async Task ShouldUpdateIndustryIfProvided()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new UpdateOrganizationCommand { Id = _orgId, IndustryId = _industryTwoId };
        var result = await SendAsync(command);
        
        result.Should().BePositive();

        var org = await FindAsync<Organization>(result, [(x => x.Industry)]);
        org.Should().NotBeNull();
        org.Name.Should().Be("Test");
        org.Industry.Should().NotBeNull();
        org.Industry.Id.Should().Be(_industryTwoId);
    }

    [Test]
    public async Task ShouldUpdateMarketSegmentIfProvided()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new UpdateOrganizationCommand { Id = _orgId, MarketSegmentId = _segmentTwoId };
        var result = await SendAsync(command);
        
        result.Should().BePositive();

        var org = await FindAsync<Organization>(result, [(x => x.MarketSegment)]);
        org.Should().NotBeNull();
        org.Name.Should().Be("Test");
        org.MarketSegment.Should().NotBeNull();
        org.MarketSegment.Id.Should().Be(_segmentTwoId);
    }

    [Test]
    public async Task ShouldUpdateLifecycleStageIfProvided()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new UpdateOrganizationCommand { Id = _orgId, LifecycleStageId = _stageTwoId };
        var result = await SendAsync(command);
        
        result.Should().BePositive();

        var org = await FindAsync<Organization>(result, [(x => x.LifecycleStage)]);
        org.Should().NotBeNull();
        org.Name.Should().Be("Test");
        org.LifecycleStage.Should().NotBeNull();
        org.LifecycleStage.Id.Should().Be(_stageTwoId);
    }

    [Test]
    public async Task ShouldThrowIfIndustryNotFound()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new UpdateOrganizationCommand { Id = _orgId, IndustryId = int.MaxValue };
        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldThrowIfMarketSegmentNotFound()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new UpdateOrganizationCommand { Id = _orgId, MarketSegmentId = int.MaxValue };
        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldThrowIfLifecycleStageNotFound()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new UpdateOrganizationCommand { Id = _orgId, LifecycleStageId = int.MaxValue };
        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldntUpdateAnythingWhenNoChangesProvided()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new UpdateOrganizationCommand { Id = _orgId };
        var result = await SendAsync(command);
        
        result.Should().Be(0);

        var org = await FindAsync<Organization>(_orgId, [(x => x.LifecycleStage), (x => x.Industry), (x => x.MarketSegment)]);
        org.Should().NotBeNull();
        org.Name.Should().Be("Test");
        org.LifecycleStage.Should().NotBeNull();
        org.LifecycleStage.Id.Should().Be(_stageId);
        org.Industry.Should().NotBeNull();
        org.Industry.Id.Should().Be(_industryId);
        org.MarketSegment.Should().NotBeNull();
        org.MarketSegment.Id.Should().Be(_segmentId);
    }

    private int _stageId;
    private int _stageTwoId;
    private int _industryId;
    private int _industryTwoId;
    private int _segmentId;
    private int _segmentTwoId;
    private int _orgId;
    protected override async Task SeedData()
    {
        await ExecuteBatchAsync(async context => {
            var stage = new LifecycleStage("Prospect");
            var stage2 = new LifecycleStage("Lead");
            
            var industry = new Industry("Manufacturing");
            var industry2 = new Industry("Biopharma");
            
            var segment = new MarketSegment("West Coast");
            var segment2 = new MarketSegment("East Coast");

            context.Add(stage);
            context.Add(stage2);
            context.Add(industry);
            context.Add(industry2);
            context.Add(segment);
            context.Add(segment2);

            await context.SaveChangesAsync();

            _stageId = stage.Id;
            _stageTwoId = stage2.Id;
            _industryId = industry.Id;
            _industryTwoId = industry2.Id;
            _segmentId = segment.Id;
            _segmentTwoId = segment2.Id;

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