using Microsoft.EntityFrameworkCore;
using DevSpector.Database;

namespace DevSpector.Tests.Database
{
    public class TestDbContext : ApplicationContextBase
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) :
            base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlite("Data Source=TestData.db");
        }
    }
}
