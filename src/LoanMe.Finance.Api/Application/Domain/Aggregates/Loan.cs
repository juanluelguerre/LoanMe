using System;
using System.ComponentModel.DataAnnotations;

namespace LoanMe.Finance.Api.Application.Domain.Aggregates
{
	public class Loan : AggregateRoot
	{
		[Key]
		public int Id { get; private set; }
		public Product Product { get; private set; }
		public string IBAN { get; private set; }
		public double Amount { get; private set; }

		public Loan(int id, Product product, string iban)
		{
			Id = id;
			Product = product;			
			IBAN = iban ?? throw new ArgumentNullException(nameof(iban));			
		}
	}
}