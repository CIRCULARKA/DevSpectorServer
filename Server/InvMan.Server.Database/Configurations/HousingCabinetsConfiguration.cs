using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.Database.Configurations
{
	public class HousingCabinetsConfiguration : IEntityTypeConfiguration<HousingCabinets>
	{
		public void Configure(EntityTypeBuilder<HousingCabinets> builder)
		{
			builder.HasKey(c => c.ID);
			builder.Property(c => c.ID).UseIdentityColumn();
			builder.Property(hc => hc.HousingID).IsRequired();
			builder.Property(hc => hc.CabinetID).IsRequired();

			// Add N/A cabinet to each housing
			var NAHousing = new List<HousingCabinets>();
			NAHousing.Add(new HousingCabinets { ID = 1, HousingID = 1, CabinetID = 1 });

			var firstHousing = new List<HousingCabinets>();
			firstHousing.Add(new HousingCabinets { ID = 2, HousingID = 2, CabinetID = 1 });

			var secondHousing = new List<HousingCabinets>();
			secondHousing.Add(new HousingCabinets { ID = 3, HousingID = 3, CabinetID = 1 });

			int i = 2;
			for (; i <= 7; i++)
				firstHousing.Add(new HousingCabinets { ID = i + 2, HousingID = 2, CabinetID = i });

			for (; i <= 13; i++)
				secondHousing.Add(new HousingCabinets { ID = i + 2, HousingID = 3, CabinetID = i });

			var result = new List<HousingCabinets>();
			result.AddRange(NAHousing);
			result.AddRange(firstHousing);
			result.AddRange(secondHousing);

			builder.HasData(result);
		}
	}
}
