using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevSpector.Domain.Models;

namespace DevSpector.Database.Configurations
{
	[ModelConfiguration]
	public class UserGroupConfiguration : IEntityTypeConfiguration<UserGroup>
	{
		public void Configure(EntityTypeBuilder<UserGroup> builder)
		{
			builder.HasKey(ug => ug.ID);
			builder.Property(ug => ug.Name).IsRequired();
		}
	}
}
