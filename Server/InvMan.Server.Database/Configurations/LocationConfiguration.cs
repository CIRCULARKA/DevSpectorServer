using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.Database.Configurations
{
	public class LocationConfiguration : IEntityTypeConfiguration<Location>
	{
		public void Configure(EntityTypeBuilder<Location> builder)
		{
			builder.HasKey(dt => dt.ID);
			builder.Property(dt => dt.ID).UseIdentityColumn();
			builder.Property(dt => dt.CabinetID).IsRequired();
			builder.Property(dt => dt.HousingID).IsRequired();
			builder.HasData(
				new Location {
					ID = 1,
					HousingID = 1,
					CabinetID = 1
				},
				new Location {
					ID = 2,
					HousingID = 2,
					// Second housing has cabinets from 4 to 9
					CabinetID = 4
				},
				new Location {
					ID = 3,
					HousingID = 3,
					// Third housing has cabinets from 10 to 15
					CabinetID = 5
				}
			);
		}
	}
}
