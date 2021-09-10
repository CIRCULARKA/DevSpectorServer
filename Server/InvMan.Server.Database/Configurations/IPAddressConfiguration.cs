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
			builder.Property(ia => ia.DeviceID).IsRequired(false);
			builder.HasIndex(ia => ia.Address).IsUnique();
			builder.Ignore(ia => ia.IsAvailable);

			builder.HasData(
				new IPAddress
				{
					ID = 1,
					Address = "198.22.33.1",
					DeviceID = 1
				},
				new IPAddress
				{
					ID = 2,
					Address = "198.22.33.2",
					DeviceID = 1
				},
				new IPAddress
				{
					ID = 3,
					Address = "198.22.33.3",
					DeviceID = 3
				},
				new IPAddress
				{
					ID = 4,
					Address = "198.22.33.4"
				},
				new IPAddress
				{
					ID = 5,
					Address = "198.22.33.5"
				}
			);
		}
	}
}
