using LoanMe.Catalog.Api.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanMe.Catalog.Api.Infrastructure.EntityConfiguration
{
	internal class CatalogItemEntityTypeConfiguration
		: IEntityTypeConfiguration<CatalogItem>
	{
		public void Configure(EntityTypeBuilder<CatalogItem> builder)
		{
			builder.ToTable("Catalog");

			builder.Property(ci => ci.Id)
				// It doesn't work in MySQL
				// .ForSqlServerUseSequenceHiLo("catalog_hilo")
				.IsRequired();

			builder.Property(ci => ci.Name)
				.IsRequired(true)
				.HasMaxLength(50);

			builder.Property(ci => ci.Price)
				.IsRequired(true);

			builder.Property(ci => ci.PictureFileName)
				.IsRequired(false);

			builder.Ignore(ci => ci.PictureUri);

			builder.HasOne(ci => ci.CatalogBrand)
				.WithMany()
				.HasForeignKey(ci => ci.CatalogBrandId);

			builder.HasOne(ci => ci.CatalogType)
				.WithMany()
				.HasForeignKey(ci => ci.CatalogTypeId);
		}
	}
}
