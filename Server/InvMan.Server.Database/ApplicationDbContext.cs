using Microsoft.EntityFrameworkCore;
using InvMan.Server.Domain.Models;
using InvMan.Server.Database.Configurations;

namespace InvMan.Server.Database
{
	public class ApplicationDbContext : ApplicationDbContextBase
	{
		private string _conntectionString;

		public ApplicationDbContext(string connectionString)
		{
			_conntectionString = connectionString;

			Database.EnsureDeleted();
			Database.EnsureCreated();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder builder) =>
			builder.UseSqlServer(_conntectionString);


		protected override void OnModelCreating(ModelBuilder builder)
		{
			ApplyModelConfigurations(builder);
		}
	}
}
