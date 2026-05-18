using FluentAssertions;
using NUnit.Framework;
using Porcupine.Application.Common.Services;
using Porcupine.Application.Industries.Commands.CreateIndustry;
using Porcupine.Domain.Entities;

namespace Porcupine.Application.UnitTests.Common.Services;

public class CommandFactoryTests
{
    private const string INDUSTRY_TEMPLATE = "{ \"Name\": \"{Name}\", \"Description\": \"{Name} Description\" }";

    [Test]
    public void CreateCommandGuardsAgainstEmptyTemplate()
    {
        var entity = new Industry("Foo");
        var act = () => CommandFactory.CreateCommand(typeof(CreateIndustryCommand), entity, "");
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void CreateCommandGuardsAgainstNonIRequestType()
    {
        var entity = new Industry("Foo");
        var act = () => CommandFactory.CreateCommand(typeof(CommandFactoryTests), entity, INDUSTRY_TEMPLATE);
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void CreateCommandCreatesCommand()
    {
        var entity = new Industry("Foo");
        var cmd = CommandFactory.CreateCommand(typeof(CreateIndustryCommand), entity, INDUSTRY_TEMPLATE);
        
        cmd.Should().NotBeNull();
        cmd.Should().BeOfType<CreateIndustryCommand>();
        
        var concreteCmd = (CreateIndustryCommand)cmd;
        concreteCmd.Description.Should().Be("Foo Description");
        concreteCmd.Name.Should().Be("Foo");
    }
}