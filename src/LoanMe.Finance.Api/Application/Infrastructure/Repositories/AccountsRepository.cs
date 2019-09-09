using LoanMe.Catalog.Api.Domain.Aggregates.AccountAggregate;
using LoanMe.Finance.Api.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;


namespace LoanMe.Catalog.Api.Application.Data
{
	public class AccountsRepository : IAccountsRepository
	{
		private readonly FinanceContext _context;

		public AccountsRepository(FinanceContext context)
		{
			_context = context;
		}

		public bool Delete(string accountNumber)
		{
			var ca = _context.Find<Account>(accountNumber);
			var result = _context.Remove(ca);
			return (result.State == EntityState.Deleted);
		}

		public bool Add(Account customerAccount)
		{
			_context.Add(customerAccount);
			var result = _context.SaveChanges();
			return result > 0;
		}

		public bool UpdateAmount(Account account, decimal amount)
		{
			int result = _context.Database.ExecuteSqlCommand("Update Account set Amount = @amount WHERE AccountId = @accountId", amount, account.AccountNumber);			
			return result > 0;
		}

		
	}
}
