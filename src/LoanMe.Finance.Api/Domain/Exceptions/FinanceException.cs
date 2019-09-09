using System;

namespace LoanMe.Finance.Api.Domain.Exceptions
{
	/// <summary>
	/// Exception type for domain exceptions
	/// </summary>
	public class FinanceException : Exception
	{
		public FinanceException()
		{ }

		public FinanceException(string message)
			: base(message)
		{ }

		public FinanceException(string message, Exception innerException)
			: base(message, innerException)
		{ }
	}
}
