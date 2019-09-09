using LoanMe.Finance.Api.Domain.ValueObjects;
using MediatR;
using System;

namespace LoanMe.Finance.Api.Doamin.Events
{
	public class TransferCreatedEvent : INotification
	{
		public AccountNumber SourceAccountNumber { get; private set; }
		public AccountNumber TargetAccountNumber { get; private set; }
		public decimal Amount { get; private set; }

		public TransferCreatedEvent(AccountNumber sourceAccountNumber, AccountNumber targetAccountNumber, decimal amount)
		{
			SourceAccountNumber = sourceAccountNumber ?? throw new ArgumentNullException(nameof(sourceAccountNumber));
			TargetAccountNumber = targetAccountNumber ?? throw new ArgumentNullException(nameof(targetAccountNumber));
			Amount = amount;
		}
	}
}
