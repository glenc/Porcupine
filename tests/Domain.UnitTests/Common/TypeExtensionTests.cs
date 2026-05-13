using FluentAssertions;
using NUnit.Framework;
using Porcupine.Domain.Common;

namespace Porcupine.Domain.UnitTests.Common;

public class TypeExtensionTests
{
    [Test]
    public void GetFriendlyNameReturnsNameAndNamespace()
    {
        typeof(object).GetFriendlyName().Should().Be("System.Object");
        typeof(int).GetFriendlyName().Should().Be("System.Int32");
        typeof(BaseEntity).GetFriendlyName().Should().Be("Porcupine.Domain.Common.BaseEntity");
    }

    [Test]
    public void GetFriendlyNameReturnsGenerics()
    {
        typeof(List<int>).GetFriendlyName().Should()
            .Be("System.Collections.Generic.List<System.Int32>");

        typeof(Dictionary<int,List<object>>).GetFriendlyName().Should()
            .Be("System.Collections.Generic.Dictionary<System.Int32, System.Collections.Generic.List<System.Object>>");
    }
}