using Xunit;
using DevSpector.Domain;
using DevSpector.Tests.Database;

namespace DevSpector.Tests
{
    public class DatabaseTestBase : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _dbFixture;

        protected TestDbContext _context;

        protected IRepository _repo;

        public DatabaseTestBase(DatabaseFixture dbFixture)
        {
            _dbFixture = dbFixture;

            _context = _dbFixture.DbContext;
            _repo = new Repository(_context);
        }
    }
}
