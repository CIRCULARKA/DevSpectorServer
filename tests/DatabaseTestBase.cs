using DevSpector.Domain;
using DevSpector.Tests.Database;

namespace DevSpector.Tests
{
    public class DatabaseTestBase
    {
        protected TestDbContext _context;

        protected IRepository _repo;

        public DatabaseTestBase()
        {
            _context = new TestDbContext();
            _repo = new Repository(_context);
        }
    }
}
