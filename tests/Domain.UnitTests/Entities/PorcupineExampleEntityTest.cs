using FluentAssertions;
using NUnit.Framework;
using Porcupine.Domain.Entities;

namespace Porcupine.Domain.UnitTests.Entities;

public class PorcupineExampleEntityTest
{
    [Test]
    public void CanCreatePorcupineExampleEntity()
    {
        var entity = new PorcupineExampleEntity();
        entity.Should().NotBeNull();
    }
}