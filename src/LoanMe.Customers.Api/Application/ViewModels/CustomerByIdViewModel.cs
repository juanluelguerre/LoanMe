using System;
using System.ComponentModel.DataAnnotations;

namespace LoanMe.Customers.Api.Application.Queries
{
	public class CustomerByIdViewModel
	{
		[Required]
		public int Id { get; set; }
		[Required]
		public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }
		public string BankAccount { get; set; }
		public bool Active { get; set; }

		public CustomerByIdViewModel(int id, string firstName, string lastName, string bankAccount, bool active)
		{
			Id = id;
			FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
			LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
			BankAccount = string.IsNullOrWhiteSpace(bankAccount) ? throw new ArgumentNullException(nameof(bankAccount)) : bankAccount;
			Active = active;
		}
	}
}