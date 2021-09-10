using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.Database.Configurations
{
	public class CabinetConfiguration : IEntityTypeConfiguration<Cabinet>
	{
		public void Configure(EntityTypeBuilder<Cabinet> builder)
		{
			builder.HasKey(c => c.ID);
			builder.Property(c => c.ID).UseIdentityColumn();
			builder.Property(c => c.Name).IsRequired();
			builder.HasData(
				new Cabinet { ID = -4, Name = "N/A ", HousingID = -1 },
				new Cabinet { ID = -5, Name = "N/A", HousingID = 1 },
				new Cabinet { ID = -6, Name = "N/A", HousingID = 2 },
				new Cabinet { ID = -1, Name = "1", HousingID = 1 },
				new Cabinet { ID = -2, Name = "201", HousingID = 2 },
				new Cabinet { ID = -3, Name = "101", HousingID = 1 }
			);
		}
	}
}
