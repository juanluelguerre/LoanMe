using LoanMe.Finance.Api.Domain.Aggregates.CustomerAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanMe.Catalog.Api.Application.EntityConfigurations
{
	internal class CreditCardEntityTypeConfiguration
		: IEntityTypeConfiguration<CreditCard>
	{
		public void Configure(EntityTypeBuilder<CreditCard> builder)
		{
			builder.ToTable("CreditCards");

			builder.HasKey(cc => cc.Id);

			builder.Property(cc => cc.Id)			   
			   .ForSqlServerUseSequenceHiLo("catalog_brand_hilo")
			   .IsRequired();

			builder.Property<int>("Number")
				.IsRequired();

			//builder.Property(cc => cc.ExpiredDate)
			//	.IsRequired();
		}
	}
}
