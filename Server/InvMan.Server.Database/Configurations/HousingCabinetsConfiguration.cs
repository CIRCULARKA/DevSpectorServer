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

			int i = 1;

			var NAHousing = new List<HousingCabinets>();
			for (; i <= 3; i++)
				NAHousing.Add(new HousingCabinets { ID = i, HousingID = 1, CabinetID = i });

			var firstHousing = new List<HousingCabinets>();
			for (; i <= 9; i++)
				firstHousing.Add(new HousingCabinets { ID = i, HousingID = 2, CabinetID = i });

			var secondHousing = new List<HousingCabinets>();
			for (; i <= 15; i++)
				secondHousing.Add(new HousingCabinets { ID = i, HousingID = 3, CabinetID = i });

			var result = new List<HousingCabinets>();
			result.AddRange(NAHousing);
			result.AddRange(firstHousing);
			result.AddRange(secondHousing);

			builder.HasData(result);
		}
	}
}
