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

		public Product(int id, string name, string description, double prize)
		{
			Id = id;
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Description = description ?? throw new ArgumentNullException(nameof(description));
			Price = prize;
			Currency = CurrencyType.EUR;
		}
	}
}
