using LoanMe.Catalog.Api.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanMe.Catalog.Api.Infrastructure.EntityConfiguration
{
	internal class CatalogBrandEntityTypeConfiguration
		: IEntityTypeConfiguration<CatalogBrand>
	{
		public void Configure(EntityTypeBuilder<CatalogBrand> builder)
		{
			builder.ToTable("CatalogBrand");

			builder.HasKey(ci => ci.Id);

			builder.Property(ci => ci.Id)
			   // It doesn't work in MySQL
			   // .ForSqlServerUseSequenceHiLo("catalog_brand_hilo")
			   .IsRequired();

			builder.Property(cb => cb.Brand)
				.IsRequired()
				.HasMaxLength(100);
		}
	}
}
