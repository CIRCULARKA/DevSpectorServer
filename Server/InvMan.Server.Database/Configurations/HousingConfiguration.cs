using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.Database.Configurations
{
	[ModelConfiguration]
	public class HousingConfiguration : IEntityTypeConfiguration<Housing>
	{
		public void Configure(EntityTypeBuilder<Housing> builder)
		{
			builder.HasKey(h => h.ID);
			builder.Property(h => h.Name).IsRequired();
		}
	}
}
