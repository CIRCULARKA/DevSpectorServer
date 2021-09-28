using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.Database.Configurations
{
	public class IPAddressConfiguration : IEntityTypeConfiguration<IPAddress>
	{
		public void Configure(EntityTypeBuilder<IPAddress> builder)
		{
			builder.HasKey(ia => ia.ID);
			builder.Property(ia => ia.ID).UseIdentityColumn();
			builder.HasIndex(ia => ia.Address).IsUnique();
			builder.HasData(
				new IPAddress { ID = 1, Address = "198.22.33.1", DeviceID = 1 },
				new IPAddress { ID = 2, Address = "198.22.33.2", DeviceID = 1 },
				new IPAddress { ID = 3, Address = "198.22.33.3", DeviceID = 1 },
				new IPAddress { ID = 4, Address = "198.22.33.4", DeviceID = 1 },
				new IPAddress { ID = 5, Address = "198.22.33.5", DeviceID = 1 },
				new IPAddress { ID = 6, Address = "198.22.33.6", DeviceID = 2 },
				new IPAddress { ID = 7, Address = "198.22.33.7", DeviceID = 2 },
				new IPAddress { ID = 8, Address = "198.22.33.8", DeviceID = 3 },
				new IPAddress { ID = 9, Address = "198.22.33.9", DeviceID = 3 },
				new IPAddress { ID = 10, Address = "198.22.33.10", DeviceID = 3 }
				// new IPAddress { ID = 11, Address = "198.22.33.11" },
				// new IPAddress { ID = 12, Address = "198.22.33.12" },
				// new IPAddress { ID = 13, Address = "198.22.33.13" }
			);
		}
	}
}
