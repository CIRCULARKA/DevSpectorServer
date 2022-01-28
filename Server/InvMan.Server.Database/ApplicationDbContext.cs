using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.Database
{
	public class ApplicationDbContext : IdentityDbContext<IdentityUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
			base(options) { }

		public DbSet<DeviceType> DeviceTypes { get; set; }

		public DbSet<Housing> Housings { get; set; }

		public DbSet<Cabinet> Cabinets { get; set; }

		public DbSet<Location> Locations { get; set; }

		public DbSet<Device> Devices { get; set; }

		public DbSet<IPAddress> IPAddresses { get; set; }

		protected void ApplyModelConfigurations(ModelBuilder builder)
		{
			builder.ApplyConfigurationsFromAssembly(
				Assembly.GetAssembly(typeof(ModelConfigurationAttribute))
			);
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			ApplyModelConfigurations(builder);
		}
	}
}
