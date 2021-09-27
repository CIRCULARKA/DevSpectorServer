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
				new Cabinet { ID = 1, Name = "N/A" },
				new Cabinet { ID = 2, Name = "N/A" },
				new Cabinet { ID = 3, Name = "N/A" },
				new Cabinet { ID = 4, Name = "1" },
				new Cabinet { ID = 5, Name = "2" },
				new Cabinet { ID = 6, Name = "3" },
				new Cabinet { ID = 7, Name = "4" },
				new Cabinet { ID = 8, Name = "5" },
				new Cabinet { ID = 9, Name = "6" },
				new Cabinet { ID = 10, Name = "7" },
				new Cabinet { ID = 11, Name = "8" },
				new Cabinet { ID = 12, Name = "9" },
				new Cabinet { ID = 13, Name = "10" },
				new Cabinet { ID = 14, Name = "11" },
				new Cabinet { ID = 15, Name = "12" }
			);
		}
	}
}
