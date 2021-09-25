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
		}
	}
}
