using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevSpector.Domain.Models;

namespace DevSpector.Database.Configurations
{
	[ModelConfiguration]
	public class DeviceIPAddressesConfiguration : IEntityTypeConfiguration<DeviceIPAddresses>
	{
		public void Configure(EntityTypeBuilder<DeviceIPAddresses> builder)
		{
			builder.HasKey(dt => dt.ID);
			builder.Property(dt => dt.DeviceID).IsRequired();
			builder.Property(dt => dt.IPAddressID).IsRequired();
		}
	}
}
