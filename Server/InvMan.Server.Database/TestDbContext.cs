using Microsoft.EntityFrameworkCore;
using InvMan.Server.Database.Configurations;

namespace InvMan.Server.Database
{
	public class TestDbContext : ApplicationDbContextBase
	{
		public TestDbContext()
		{
			Database.EnsureDeleted();
			Database.EnsureCreated();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder builder) =>
			builder.UseSqlServer("Server=(local);Database=TestInvManDb;Trusted_Connection=true;");
		protected override void OnModelCreating(ModelBuilder builder)
		{
			ApplyModelConfigurations(builder);
		}
	}
}
