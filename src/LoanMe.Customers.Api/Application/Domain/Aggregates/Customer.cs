using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LoanMe.Customers.Api.Application.Domain.Aggregates
{
	public class Customer : AggregateRoot
	{
		[Key]
		public int Id { get; set; }
		[MaxLength(50)]
		public string FirstName { get; private set; }
		[MaxLength(100)]
		public string LastName { get; private set; }
		// public DateTime? CustomerFrom { get; private set; }		
		public bool Active { get; private set; }	
		public ICollection<CustomerAccount> BankAccounts { get; set; }
		// public string BankAccount { get; set; }

		//public Customer(int id, string firstName, string lastName, DateTime? customerFrom, string bankAccount)
		//{
		//	Id = id;
		//	FirstName = firstName;
		//	LastName = lastName;
		//	CustomerFrom = customerFrom;
		//	BankAccounts = new List<CustomerAccount>();
		//	if (!string.IsNullOrWhiteSpace(bankAccount))
		//	{
		//		BankAccounts.Add(new CustomerAccount(id, bankAccount, true));
		//	}			
		//}

		//public Customer()
		//{
		//	BankAccounts = new List<CustomerAccount>();
		//}

		// No parametrized constructor used to avoid innecesary properties
		public static Customer CreateNew(int id, string firstName, string lastName)
		{
			// return new Customer(id, firstName, lastName, customerFrom, bankAccount);
			var c = new Customer
			{
				Id = id,
				FirstName = firstName,
				LastName = lastName,				
				Active = true,
				BankAccounts = new List<CustomerAccount>()
			};			
			//if (!string.IsNullOrWhiteSpace(bankAccount))
			//{
			//	c.BankAccounts.Add(new CustomerAccount(id, bankAccount, true));
			//}
			return c;
		}

		//public static Customer CreateNew(int id, string firstName, string lastName, DateTime? customerFrom)
		//{
		//	// return new Customer(id, firstName, lastName, customerFrom, null);
		//	return CreateNew(id, firstName, lastName, customerFrom, null);
		//}
	}
}
