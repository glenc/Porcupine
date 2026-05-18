using Porcupine.Application.Common.NotificationHandlers;
using Porcupine.Application.Common.Services;
using Porcupine.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using Porcupine.Domain.Entities;
using MediatR;
using Porcupine.Domain.Triggers;
using Porcupine.Infrastructure.Data;
using Porcupine.Application.Industries.Commands.CreateIndustry;
using Porcupine.Domain.ValueObjects;

namespace Porcupine.Application.FunctionalTests.NotificationHandlers;

using static Testing;

public class EventTriggerHandlerTests_MockCommands : BaseTestFixture
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
            It.Is<object>(o => o is CreateIndustryCommand),
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
            It.Is<object>(o => o is CreateIndustryCommand),
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
            It.Is<object>(o => o is CreateIndustryCommand),
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
            It.Is<object>(o => o is CreateIndustryCommand),
            It.IsAny<CancellationToken>()
        ), Times.Exactly(3));
    }

    private const string CREATE_INDUSTRY_TEMPLATE = "{ \"Name\": \"{Name} Copy\", \"Description\": \"{Description} Copy\" }";
    protected override async Task SeedData()
    {
        var rule = Rule.DomainEventRuleFor<IndustryUpdatedEvent>("IndustryUpdate_NoCriteria");
        rule.AddAction(RuleAction.For<CreateIndustryCommand>(CREATE_INDUSTRY_TEMPLATE));
        await AddAsync<Rule>(rule);

        rule = Rule.DomainEventRuleFor<IndustryUpdatedEvent>("IndustryUpdate_NameCheck", "Name == \"Test\"");
        rule.AddAction(RuleAction.For<CreateIndustryCommand>(CREATE_INDUSTRY_TEMPLATE));
        await AddAsync<Rule>(rule);

        rule = Rule.DomainEventRuleFor<IndustryUpdatedEvent>("IndustryUpdate_NameCheck", "Name == \"TestTwo\"");
        rule.AddAction(RuleAction.For<CreateIndustryCommand>(CREATE_INDUSTRY_TEMPLATE));
        await AddAsync<Rule>(rule);

        rule = Rule.DomainEventRuleFor<IndustryUpdatedEvent>("IndustryUpdate_NameCheck", "Name == \"TestTwo\" || Name == \"TestThree\"");
        rule.AddAction(RuleAction.For<CreateIndustryCommand>(CREATE_INDUSTRY_TEMPLATE));
        await AddAsync<Rule>(rule);

        await base.SeedData();
    }
}