using LoanMe.Finance.Api.Domain.ValueObjects;
using MediatR;
using System;

namespace LoanMe.Finance.Api.Doamin.Events
{
	public class DepositAddedEvent : INotification
	{
		public AccountNumber AccountNumber { get; private set; }
		public decimal Amount { get; private set; }

		public DepositAddedEvent(AccountNumber accountNumber, decimal amount)
		{
			AccountNumber = accountNumber ?? throw new ArgumentNullException(nameof(accountNumber));
			Amount = amount;
		}
	}
}
