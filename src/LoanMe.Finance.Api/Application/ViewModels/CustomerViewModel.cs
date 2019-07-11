using System;

namespace LoanMe.Catalog.Api.Application.Queries
{
	public class CustomerViewModel
	{
		public int Id { get; private set; }
		public string FirstName { get; private set; }
		public string LastName { get; private set; }
		public string BankAccount { get; private set; }
		public bool MarkAsDefault { get; private set; }

		// BUG:	https://github.com/StackExchange/Dapper/issues/456
		// A parameterless default constructor or one matching signature(System.Int32 Id, System.String FirstName, System.String LastName, System.String BankAccount, System.Boolean MarkAsDefault) 
		// is required for LoanMe.Customers.Api.Application.Queries.CustomerViewModel materialization
		private  CustomerViewModel() { }		

		public CustomerViewModel(int id, string firtName, string lastName, string bankAccount, bool markAsDefault)
		{
			Id = id;
			FirstName = firtName;
			LastName = lastName;
			BankAccount = bankAccount;
			MarkAsDefault = markAsDefault;
			// BankAccount = string.IsNullOrWhiteSpace(bankAccount) ? throw new ArgumentNullException(nameof(bankAccount)) : bankAccount;
		}
	}
}