using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.Database.Configurations
{
	public class DeviceConfiguration : IEntityTypeConfiguration<Device>
	{
		public void Configure(EntityTypeBuilder<Device> builder)
		{
			builder.HasKey(d => d.ID);
			builder.Property(d => d.ID).UseIdentityColumn();
			builder.HasIndex(d => d.InventoryNumber).IsUnique();
			builder.Property(d => d.DeviceTypeID).IsRequired();
			builder.Property(d => d.NetworkName).IsRequired();
			builder.Property(d => d.CabinetID).IsRequired();
			builder.Property(d => d.CabinetID).HasDefaultValue(-4);
			builder.HasData(
				new Device
				{
					ID = 1,
					InventoryNumber = "NSGK530923",
					DeviceTypeID = 1,
					NetworkName = "IVAN-PC",
					CabinetID = -3
				},
				new Device
				{
					ID = 2,
					InventoryNumber = "NSGK052132",
					DeviceTypeID = 2,
					NetworkName = "MAIN-SERVER",
					CabinetID = -2
				},
				new Device
				{
					ID = 3,
					InventoryNumber = "NSGK1235231",
					DeviceTypeID = 3,
					NetworkName = "COMMUTATOR-1",
					CabinetID = -1
				}
			);
		}
	}
}
