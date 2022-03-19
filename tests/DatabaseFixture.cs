using System;

namespace DevSpector.Tests.Database
{

    public class DatabaseFixture : IDisposable
    {
        private TestDbContext _context;

        public DatabaseFixture()
        {
            _context = new TestDbContext();
        }

        public TestDbContext DbContext => _context;

        public void Dispose() =>
            _context.Dispose();
    }
}
