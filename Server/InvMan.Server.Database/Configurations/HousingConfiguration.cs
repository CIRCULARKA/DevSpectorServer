using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.Database.Configurations
{
	public class HousingConfiguration : IEntityTypeConfiguration<Housing>
	{
		public void Configure(EntityTypeBuilder<Housing> builder)
		{
			builder.HasKey(h => h.ID);
			builder.Property(h => h.ID).UseIdentityColumn();
			builder.Property(h => h.Name).IsRequired();
			builder.HasData(
					new Housing { ID = -1, Name = "N/A" },
					new Housing { ID = 1, Name = "Главный" },
					new Housing { ID = 2, Name = "Второй" }
			);
		}
	}
}
