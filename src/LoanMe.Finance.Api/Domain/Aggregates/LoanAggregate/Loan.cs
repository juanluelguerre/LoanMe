using LoanMe.Finance.Api.Domain.ValueObjects;
using System;

namespace LoanMe.Finance.Api.Domain.Aggregates
{
	public class Loan : Entity, IAggregateRoot
	{		
		public int ProductId { get; set; }		
		public AccountNumber AccountNumber { get;  set; }		
		public decimal Amount { get; set; }

		public Loan(int id, int productId, AccountNumber accountNumber)
		{
			Id = id > 0 ? id: throw new ArgumentException($"LoanId must be greater than 0 !");
			ProductId = productId > 0 ? productId : throw new ArgumentException($"ProductId must be greater than 0 !"); 
			AccountNumber = accountNumber ?? throw new ArgumentNullException(nameof(accountNumber));
			Amount = 0M;
		}
	}
}