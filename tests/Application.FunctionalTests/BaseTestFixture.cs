namespace Porcupine.Application.FunctionalTests;

using static Testing;

[TestFixture]
public abstract class BaseTestFixture
{
    [SetUp]
    public async Task TestSetUp()
    {
        await ResetState();
        await SeedData();
    }

    protected virtual async Task SeedData()
    {
        await Task.Yield();
    }
}
