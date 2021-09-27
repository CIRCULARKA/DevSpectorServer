using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.Database.Configurations
{
	public class DeviceTypeConfiguration : IEntityTypeConfiguration<DeviceType>
	{
		public void Configure(EntityTypeBuilder<DeviceType> builder)
		{
			builder.HasKey(dt => dt.ID);
			builder.Property(dt => dt.ID).UseIdentityColumn();
			builder.Property(dt => dt.Name).IsRequired();
			builder.HasData(
				new DeviceType { ID = 1, Name = "Персональный компьютер" },
				new DeviceType { ID = 2, Name = "Сервер" },
				new DeviceType { ID = 3, Name = "Коммутатор" }
			);
		}
	}
}
