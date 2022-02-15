using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevSpector.Domain.Models;

namespace DevSpector.Database.Configurations
{
	[ModelConfiguration]
	public class DeviceTypeConfiguration : IEntityTypeConfiguration<DeviceType>
	{
		public void Configure(EntityTypeBuilder<DeviceType> builder)
		{
			builder.HasKey(dt => dt.ID);
			builder.Property(dt => dt.Name).IsRequired();
		}
	}
}
