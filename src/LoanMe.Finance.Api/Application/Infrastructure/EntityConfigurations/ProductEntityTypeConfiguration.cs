using LoanMe.Finance.Api.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanMe.Catalog.Api.Application.EntityConfigurations
{
	internal class ProductEntityTypeConfiguration
		: IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{
			builder.ToTable("Products");

			builder.HasKey(p => p.Id);

			builder.Property(p => p.Id)
			   .ForSqlServerUseSequenceHiLo("catalog_brand_hilo")
			   .IsRequired();

			builder.Property(p => p.Name)
				.IsRequired();

			builder.Property(p => p.Price)
				.IsRequired();
		}
	}
}
