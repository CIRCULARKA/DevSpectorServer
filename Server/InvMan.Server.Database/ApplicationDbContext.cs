using Microsoft.EntityFrameworkCore;

namespace InvMan.Server.Database
{
	public class ApplicationDbContext : ApplicationDbContextBase
	{
		private string _conntectionString;

		public ApplicationDbContext(string connectionString)
		{
			_conntectionString = connectionString;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder builder) =>
			builder.UseSqlServer(_conntectionString);


		protected override void OnModelCreating(ModelBuilder builder)
		{
			ApplyModelConfigurations(builder);
		}
	}
}
