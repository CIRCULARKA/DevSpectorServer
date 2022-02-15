using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevSpector.Domain.Models;

namespace DevSpector.Database.Configurations
{
	[ModelConfiguration]
	public class LocationConfiguration : IEntityTypeConfiguration<Location>
	{
		public void Configure(EntityTypeBuilder<Location> builder)
		{
			builder.HasKey(dt => dt.ID);
			builder.Property(dt => dt.CabinetID).IsRequired();
			builder.Property(dt => dt.HousingID).IsRequired();
		}
	}
}
