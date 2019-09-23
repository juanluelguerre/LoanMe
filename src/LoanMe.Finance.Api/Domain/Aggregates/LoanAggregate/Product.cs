using LoanMe.Finance.Api.Domain.Aggregates.LoanAggregate;
using System;

namespace LoanMe.Finance.Api.Domain.Aggregates
{
	public class Product : Entity
	{
		public string Name { get; private set; }
		public string Description { get; private set; }
		public double Price { get; private set; }
		public CurrencyType Currency { get; private set; }

		protected Product()
		{
			Currency = CurrencyType.EUR;
		}

		public Product(string name, string description, double price) : this()
		{			
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Description = description ?? throw new ArgumentNullException(nameof(description));
			Price = price;			
		}
	}
}
