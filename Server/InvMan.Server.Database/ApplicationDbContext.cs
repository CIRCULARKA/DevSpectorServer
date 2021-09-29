using Microsoft.EntityFrameworkCore;

namespace InvMan.Server.Database
{
	public class ApplicationDbContext : ApplicationDbContextBase
	{
		protected override void OnModelCreating(ModelBuilder builder)
		{
			ApplyModelConfigurations(builder);
		}
	}
}
