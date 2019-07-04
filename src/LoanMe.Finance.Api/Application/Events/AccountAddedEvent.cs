using MediatR;
using System;

namespace LoanMe.Finance.Api.Application.Events
{
	public class AccountAddedEvent : INotification
	{
		public string IBAN { get; private set; }

		public AccountAddedEvent(string iban)
		{
			if (string.IsNullOrWhiteSpace(iban)) throw new ArgumentException(nameof(iban));
			IBAN = iban;
		}		
	}
}
