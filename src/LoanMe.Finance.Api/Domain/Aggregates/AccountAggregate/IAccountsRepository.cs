using LoanMe.Finance.Api.Domain.Aggregates;

namespace LoanMe.Catalog.Api.Domain.Aggregates.AccountAggregate
{
	public interface IAccountsRepository
	{
		bool Add(Account customerAccount);
		bool Delete(string accountNumber);		
		bool UpdateAmount(Account account, decimal amount);
	}
}