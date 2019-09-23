using LoanMe.Finance.Api.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanMe.Catalog.Api.Application.EntityConfigurations
{
	internal class AccountEntityTypeConfiguration
		: IEntityTypeConfiguration<Account>
	{
		public void Configure(EntityTypeBuilder<Account> builder)
		{
			builder.ToTable("Accounts");

			builder.HasKey(a => a.Id);

			builder.Property(a => a.Id)			   
			   .ForSqlServerUseSequenceHiLo("catalog_brand_hilo")
			   .IsRequired();

			//CreditCard value object persisted as owned entity type supported since EF Core 2.0
			builder.OwnsOne(a => a.AccountNumber);
			builder.OwnsOne(a => a.CreditCard);
			builder.OwnsOne(a => a.Customer);

			//builder.Property(a => a.AccountNumber)
			//	.IsRequired();

			//builder.Property(a => a.Customer)
			//	.IsRequired();				

			var navigationCreditCard = builder.Metadata.FindNavigation(nameof(Account.CreditCard));

			// DDD Patterns comment:
			// Set as field (New since EF 1.1) to access the OrderItem collection property through its field
			navigationCreditCard.SetPropertyAccessMode(PropertyAccessMode.Field);

			var navigationCustomer = builder.Metadata.FindNavigation(nameof(Account.Customer));

			// DDD Patterns comment:
			// Set as field (New since EF 1.1) to access the OrderItem collection property through its field
			navigationCustomer.SetPropertyAccessMode(PropertyAccessMode.Field);

		}
	}
}
