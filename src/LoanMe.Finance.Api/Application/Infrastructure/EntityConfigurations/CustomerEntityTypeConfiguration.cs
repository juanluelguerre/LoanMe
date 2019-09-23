using LoanMe.Finance.Api.Domain.Aggregates.CustomerAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanMe.Catalog.Api.Application.EntityConfigurations
{
	internal class CustomerEntityTypeConfiguration
		: IEntityTypeConfiguration<Customer>
	{
		public void Configure(EntityTypeBuilder<Customer> builder)
		{
			builder.ToTable("Customers");

			builder.HasKey(c => c.Id);

			builder.Property(c => c.Id)			   
			   .ForSqlServerUseSequenceHiLo("catalog_brand_hilo")
			   .IsRequired();

		}
	}
}
