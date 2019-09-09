using System;

namespace LoanMe.Finance.Api.Domain.Aggregates.CustomerAggregate
{
	public class Customer : Entity
	{
		public string Name { get; private set; }
		
		public Customer(string name)
		{
			Name = name ?? throw new ArgumentNullException(nameof(name));			
		}
	}
}
