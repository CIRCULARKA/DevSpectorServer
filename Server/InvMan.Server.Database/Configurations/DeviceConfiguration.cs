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
			builder.Property(d => d.LocationID).IsRequired();
			builder.Property(d => d.InventoryNumber).IsRequired();
			builder.HasIndex(d => d.InventoryNumber).IsUnique();

			int typePC = 1, typeServer = 2, typeCommutator = 3;
			builder.HasData(
				new Device {
					ID = 1,
					InventoryNumber = "NSGK530923",
					NetworkName = "IVAN-PC",
					TypeID = typePC,
					LocationID = 1
				},
				new Device {
					ID = 2,
					InventoryNumber = "NSGK654212",
					NetworkName = "MAIN-SERVER",
					TypeID = typeServer,
					LocationID = 2
				},
				new Device {
					ID = 3,
					InventoryNumber = "NSGK1235231",
					NetworkName = "COMMUTATOR-1",
					TypeID = typeCommutator,
					LocationID = 3
				}
			);
		}
	}
}
