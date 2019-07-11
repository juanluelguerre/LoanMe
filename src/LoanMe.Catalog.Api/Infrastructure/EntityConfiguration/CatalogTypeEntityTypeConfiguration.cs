using LoanMe.Catalog.Api.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanMe.Catalog.Api.Infrastructure.EntityConfiguration
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
