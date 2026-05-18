namespace Porcupine.Application.FunctionalTests.NotificationHandlers;

using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Porcupine.Application.Common.NotificationHandlers;
using Porcupine.Application.Industries.Commands.CreateIndustry;
using Porcupine.Application.MarketSegments.Commands.CreateMarketSegment;
using Porcupine.Domain.Entities;
using Porcupine.Domain.Events;
using Porcupine.Domain.ValueObjects;
using Porcupine.Infrastructure.Data;
using static Testing;

public class EventTriggerHandlerTests_RealCommands : BaseTestFixture
{
    private const string CREATE_INDUSTRY_TEMPLATE = "{ \"Name\": \"{Name} Copy\", \"Description\": \"{Description} Copy\" }";
    private const string CREATE_MARKETSEGMENT_TEMPLATE = "{ \"Name\": \"{Name} Industry\", \"Description\": \"Industry: {Description}\" }";

    [Test]
    public async Task CommandsArePublishedWithCorrectValues()
    {
        using var scope = CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var sender = new Mock<ISender>();

        var handler = new IndustryEventTriggerHandler<IndustryCreatedEvent>(context, sender.Object);

        await handler.Handle(new IndustryCreatedEvent(new Industry("Test", "Cool Industry")), CancellationToken.None);

        sender.Verify(x => x.Send(
            It.Is<object>(o => 
                o is CreateIndustryCommand &&
                ((CreateIndustryCommand)o).Name == "Test Copy" &&
                ((CreateIndustryCommand)o).Description == "Cool Industry Copy"),
            It.IsAny<CancellationToken>()
        ), Times.Exactly(1));

        sender.Verify(x => x.Send(
            It.Is<object>(o => o is CreateMarketSegmentCommand &&
                ((CreateMarketSegmentCommand)o).Name == "Test Industry" &&
                ((CreateMarketSegmentCommand)o).Description == "Industry: Cool Industry"),
            It.IsAny<CancellationToken>()
        ), Times.Exactly(1));
    }

    protected override async Task SeedData()
    {
        var rule = Rule.DomainEventRuleFor<IndustryCreatedEvent>("IndustryCreated_NoCriteria");
        rule.AddAction(RuleAction.For<CreateIndustryCommand>(CREATE_INDUSTRY_TEMPLATE));
        rule.AddAction(RuleAction.For<CreateMarketSegmentCommand>(CREATE_MARKETSEGMENT_TEMPLATE));

        await AddAsync<Rule>(rule);

        await base.SeedData();
    }
}