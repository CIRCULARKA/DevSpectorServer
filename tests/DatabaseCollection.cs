using Xunit;

namespace DevSpector.Tests.Database
{
    [CollectionDefinition(nameof(DatabaseCollection))]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture> { }
}
