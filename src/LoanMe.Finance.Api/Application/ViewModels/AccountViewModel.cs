using LoanMe.Finance.Api.Domain.Aggregates;
using System;

namespace LoanMe.Finance.Api.Application.Queries
{
	public class AccountViewModel
	{		
		public Account Account { get; private set; }
		

		// BUG:	https://github.com/StackExchange/Dapper/issues/456
		// A parameterless default constructor or one matching signature(System.Int32 Id, System.String FirstName, System.String LastName, System.String BankAccount, System.Boolean MarkAsDefault) 
		// is required for LoanMe.Customers.Api.Application.Queries.CustomerViewModel materialization
		private  AccountViewModel() { }

		public AccountViewModel(Account account)
		{
			Account = account ?? throw new ArgumentNullException(nameof(account));
		}
	}
}