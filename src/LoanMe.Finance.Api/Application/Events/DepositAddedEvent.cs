using MediatR;
using System;

namespace LoanMe.Finance.Api.Application.Events
{
	public class DepositAddedEvent : INotification
	{
		public string IBAN { get; private set; }

		public DepositAddedEvent(string iban)
		{
			if (string.IsNullOrWhiteSpace(iban)) throw new ArgumentException(nameof(iban));
			IBAN = iban;
		}		
	}
}
