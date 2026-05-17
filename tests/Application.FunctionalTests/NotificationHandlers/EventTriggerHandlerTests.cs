using Porcupine.Application.Common.NotificationHandlers;
using Porcupine.Application.Common.Services;
using Porcupine.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using Porcupine.Domain.Entities;
using MediatR;
using Porcupine.Domain.Triggers;
using Porcupine.Infrastructure.Data;
using Porcupine.Application.Industries.Commands.CreateIndustry;

namespace Porcupine.Application.FunctionalTests.NotificationHandlers;

using static Testing;

public class EventTriggerHandlerTests : BaseTestFixture
{
    [Test]
    public async Task NoCommandsArePublishedWhenNoMatchingRulesAreFound()
    {
        using var scope = CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var sender = Mock.Of<ISender>();

        var handler = new IndustryEventTriggerHandler<IndustryCreatedEvent>(context, sender);

        await handler.Handle(new IndustryCreatedEvent(new Industry("Test")), CancellationToken.None);

        Mock.Get(sender).Verify(x => x.Send(
            It.IsAny<FakeAction>(),
            It.IsAny<CancellationToken>()
        ), Times.Never);
    }

    [Test]
    public async Task OnlyPublishCommandsForMatchingCriteria_NoCriteriaMatch()
    {
        using var scope = CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var sender = Mock.Of<ISender>();

        var handler = new IndustryEventTriggerHandler<IndustryUpdatedEvent>(context, sender);

        var changes = new Dictionary<string, object?>{["Name"] = "Test"};
        
        await handler.Handle(new IndustryUpdatedEvent(new Industry("Not Test"), changes), CancellationToken.None);

        Mock.Get(sender).Verify(x => x.Send(
            It.IsAny<FakeAction>(),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }

    [Test]
    public async Task OnlyPublishCommandsForMatchingCriteria_OneCriteriaMatch()
    {
        using var scope = CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var sender = Mock.Of<ISender>();

        var handler = new IndustryEventTriggerHandler<IndustryUpdatedEvent>(context, sender);

        var changes = new Dictionary<string, object?>{["Name"] = "Old Test"};
        
        await handler.Handle(new IndustryUpdatedEvent(new Industry("Test"), changes), CancellationToken.None);

        Mock.Get(sender).Verify(x => x.Send(
            It.IsAny<FakeAction>(),
            It.IsAny<CancellationToken>()
        ), Times.Exactly(2));
    }

    [Test]
    public async Task OnlyPublishCommandsForMatchingCriteria_TwoCriteriaMatch()
    {
        using var scope = CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var sender = Mock.Of<ISender>();

        var handler = new IndustryEventTriggerHandler<IndustryUpdatedEvent>(context, sender);

        var changes = new Dictionary<string, object?>{["Name"] = "Old Test"};
        
        await handler.Handle(new IndustryUpdatedEvent(new Industry("TestTwo"), changes), CancellationToken.None);

        Mock.Get(sender).Verify(x => x.Send(
            It.IsAny<FakeAction>(),
            It.IsAny<CancellationToken>()
        ), Times.Exactly(3));
    }

    protected override async Task SeedData()
    {
        await AddAsync<Rule>(Rule.DomainEventRuleFor<IndustryUpdatedEvent>("IndustryUpdate_NoCriteria"));
        await AddAsync<Rule>(Rule.DomainEventRuleFor<IndustryUpdatedEvent>("IndustryUpdate_NameCheck", "Name == \"Test\""));
        await AddAsync<Rule>(Rule.DomainEventRuleFor<IndustryUpdatedEvent>("IndustryUpdate_NameCheck", "Name == \"TestTwo\""));
        await AddAsync<Rule>(Rule.DomainEventRuleFor<IndustryUpdatedEvent>("IndustryUpdate_NameCheck", "Name == \"TestTwo\" || Name == \"TestThree\""));

        await base.SeedData();
    }
}