using System;

namespace LoanMe.Finance.Api.Domain.Aggregates.CustomerAggregate
{
	public class CreditCard : Entity
	{
		public int Number { get; private set; }		
		public DateTime ExpiredDate { get; private set; }
		public CardType CardType { get; private set; }
		public decimal Limit { get; private set; }

		public CreditCard(int number, DateTime expiredDate, CardType cardType, decimal limit)
		{
			Number = number > 0 ? number : throw new ArgumentException(nameof(number));
			ExpiredDate = expiredDate;
			CardType = cardType;
			Limit = limit >= 0 ? limit : throw new ArgumentException(nameof(limit));
		}
	}
}
