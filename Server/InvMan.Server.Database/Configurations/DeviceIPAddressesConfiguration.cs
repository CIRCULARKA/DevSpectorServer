using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.Database.Configurations
{
	public class DeviceIPAddressesConfiguration : IEntityTypeConfiguration<DeviceIPAddresses>
	{
		public void Configure(EntityTypeBuilder<DeviceIPAddresses> builder)
		{
			builder.HasKey(dt => dt.ID);
			builder.Property(dt => dt.ID).UseIdentityColumn();
			builder.Property(dt => dt.DeviceID).IsRequired();
			builder.Property(dt => dt.IPAddressID).IsRequired();
			var firstDeviceIPs = new List<DeviceIPAddresses>();
			int i = 1;
			for (; i <= 3; i++)
				firstDeviceIPs.Add(
					new DeviceIPAddresses { ID = i, DeviceID = 1, IPAddressID = i }
				);

			var secondDeviceIPs = new List<DeviceIPAddresses>();
			for (; i <= 5; i++)
				secondDeviceIPs.Add(
					new DeviceIPAddresses { ID = i, DeviceID = 2, IPAddressID = i }
				);

			var thirdDeviceIPs = new List<DeviceIPAddresses>();
			for (; i <= 9; i++)
				thirdDeviceIPs.Add(
					new DeviceIPAddresses { ID = i, DeviceID = 3, IPAddressID = i }
				);

			var result = new List<DeviceIPAddresses>();
			result.AddRange(firstDeviceIPs);
			result.AddRange(secondDeviceIPs);
			result.AddRange(thirdDeviceIPs);
			builder.HasData(result);
		}
	}
}
