using Xunit;

namespace DevSpector.Tests.Database
{
    [CollectionDefinition("DbCollection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture> { }
}
