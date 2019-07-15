using System;
using System.ComponentModel.DataAnnotations;

namespace LoanMe.Finance.Api.Application.Domain.Aggregates
{
	public class Loan : AggregateRoot
	{
		[Key]
		public int LoanId { get;  set; }
		// public Product Product { get;  set; }
		[Required]
		public int ProductId { get; set; }
		[Required]
		public string IBAN { get;  set; }

		public Loan(int loanId, int productId, string iban)
		{
			LoanId = loanId > 0 ? loanId: throw new ArgumentException($"LoanId must be greater than 0 !");
			ProductId = productId > 0 ? productId : throw new ArgumentException($"ProductId must be greater than 0 !"); 
			IBAN = iban ?? throw new ArgumentNullException(nameof(iban));
		}
	}
}