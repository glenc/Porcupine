using Porcupine.Application.Common.Exceptions;
using Porcupine.Application.Industries.Commands.CreateIndustry;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.FunctionalTests.Industries.Commands;

using static Testing;

public class CreateIndustryTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new CreateIndustryCommand { };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
        
        command = new CreateIndustryCommand { Description = "asdf" };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireAuthentication() 
    {
        var command = new CreateIndustryCommand { Name = "foo" };

        await FluentActions.Invoking(() => SendAsync(command))
            .Should().ThrowAsync<UnauthorizedAccessException>();
    }
    
    [Test]
    public async Task ShouldRequireAdministratorRole() 
    {
        await ResetState();
        var userId = await RunAsDefaultUserAsync();

        var query = new CreateIndustryCommand { Name = "foo" };

        await FluentActions.Invoking(() => SendAsync(query))
            .Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task ShouldReturnCreateIndustry()
    {
        var userId = await RunAsAdministratorAsync();

        var command = new CreateIndustryCommand { Name = "foo", Description = "bar" };
        var result = await SendAsync(command);
        
        result.Should().BePositive();

        var industry = await FindAsync<Industry>(result);
        industry.Should().NotBeNull();
        industry.Name.Should().Be("foo");
        industry.Description.Should().Be("bar");
    }

    protected override async Task SeedData()
    {
        await base.SeedData();
    }
}