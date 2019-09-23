using LoanMe.Finance.Api.Domain.Aggregates.CustomerAggregate;
using LoanMe.Finance.Api.Domain.ValueObjects;

namespace LoanMe.Finance.Api.Domain.Aggregates
{
	public class Account : Entity, IAggregateRoot
	{		
		public AccountNumber AccountNumber { get; private set; }
		public Customer Customer { get; private set; }
		public decimal Balance { get; private set; }		
		public CreditCard CreditCard { get; private set; }

		protected Account()
		{
		}

		public Account(AccountNumber accountNumber, Customer customer, CreditCard creditCard, decimal balance) : this()
		{			
			AccountNumber = accountNumber;
			Customer = customer;
			CreditCard = creditCard;
			Balance = balance;
		}

		//public void AddAmount(decimal amount)
		//{
		//	Balance += amount;
		//}

		//public void RemoveAmount(decimal amount)
		//{
		//	Balance -= amount;
		//}		
	}
}

