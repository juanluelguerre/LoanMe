using System;
using LoanMe.Finance.Api.Domain.ValueObjects;

namespace LoanMe.Finance.Api.Domain.Aggregates.BankAggregate
{
	public class Bank : Entity, IAggregateRoot
	{
		public string Name { get; private set; }
		public string EmailContact { get; private set; }
		public Address Address { get; private set; }

		public Bank(string name, string emailContact, Address address)
		{
			Name = name ?? throw new ArgumentNullException(nameof(name));
			EmailContact = emailContact ?? throw new ArgumentNullException(nameof(emailContact));
			Address = address ?? throw new ArgumentNullException(nameof(address));
		}		
	}
}
