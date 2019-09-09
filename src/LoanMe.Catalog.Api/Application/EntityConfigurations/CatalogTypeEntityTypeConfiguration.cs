using LoanMe.Catalog.Api.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanMe.Catalog.Api.Application.EntityConfigurations
{
	internal class CatalogTypeEntityTypeConfiguration
	   : IEntityTypeConfiguration<CatalogType>
	{
		public void Configure(EntityTypeBuilder<CatalogType> builder)
		{
			builder.ToTable("CatalogType");

			builder.HasKey(ci => ci.Id);

			builder.Property(ci => ci.Id)
			   // It doesn't work in MySQL
			   .ForSqlServerUseSequenceHiLo("catalog_type_hilo")
			   .IsRequired();

			builder.Property(cb => cb.Type)
				.IsRequired()
				.HasMaxLength(100);
		}
	}
}
