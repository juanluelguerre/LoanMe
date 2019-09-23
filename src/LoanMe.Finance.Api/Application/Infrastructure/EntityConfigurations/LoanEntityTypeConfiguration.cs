using LoanMe.Finance.Api.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanMe.Catalog.Api.Application.EntityConfigurations
{
	internal class LoanEntityTypeConfiguration
		: IEntityTypeConfiguration<Loan>
	{
		public void Configure(EntityTypeBuilder<Loan> builder)
		{
			builder.ToTable("Loans");

			builder.HasKey(l => l.Id);

			builder.Property(l => l.Id)
			   .ForSqlServerUseSequenceHiLo("catalog_brand_hilo")
			   .IsRequired();

			//CreditCard value object persisted as owned entity type supported since EF Core 2.0
			builder.OwnsOne(a => a.AccountNumber);

			builder.Property(l => l.ProductId)
				.IsRequired();

			builder.Property(l => l.Amount)
				.IsRequired()
				.HasDefaultValue(0);
		}	
	}
}
