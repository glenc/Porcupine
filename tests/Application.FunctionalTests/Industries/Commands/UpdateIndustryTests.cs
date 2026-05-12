using Porcupine.Application.Common.Exceptions;
using Porcupine.Application.Industries.Commands.UpdateIndustry;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.FunctionalTests.Industries.Commands;

using static Testing;

public class UpdateIndustryTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new UpdateIndustryCommand { };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireAuthentication() 
    {
        var command = new UpdateIndustryCommand { Id = 1 };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<UnauthorizedAccessException>();
    }
    
    [Test]
    public async Task ShouldRequireAdministratorRole() 
    {
        await ResetState();
        var userId = await RunAsDefaultUserAsync();

        var query = new UpdateIndustryCommand { Id = 1 };

        await FluentActions.Invoking(() => SendAsync(query))
            .Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task ShouldUpdateIndustry()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new UpdateIndustryCommand { Id = _id, Name = "New Name", Description = "New Description" };
        var result = await SendAsync(command);
        
        result.Should().BePositive();

        var industry = await FindAsync<Industry>(_id);
        industry.Should().NotBeNull();
        industry.Name.Should().Be("New Name");
        industry.Description.Should().Be("New Description");
    }

    [Test]
    public async Task ShouldThrowIfNotFound()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new UpdateIndustryCommand { Id = int.MaxValue, Name = "New Name", Description = "New Description" };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<NotFoundException>();
    }

    private int _id;
    protected override async Task SeedData()
    {
        var entity = new Industry("foo", "bar");
        await AddAsync<Industry>(entity);
        _id = entity.Id;

        await base.SeedData();
    }
}