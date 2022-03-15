using Microsoft.EntityFrameworkCore;

namespace DevSpector.Database
{
	public class ApplicationDbContext : ApplicationContextBase
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
			base(options) { }
	}
}
