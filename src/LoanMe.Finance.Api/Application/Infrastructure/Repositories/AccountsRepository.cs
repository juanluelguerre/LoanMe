using LoanMe.Catalog.Api.Domain.Aggregates.AccountAggregate;
using LoanMe.Finance.Api.Domain.Aggregates;
using LoanMe.Finance.Api.Domain.Aggregates.CustomerAggregate;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LoanMe.Catalog.Api.Application.Data
{
	public class AccountsRepository : IAccountsRepository
	{
		private readonly FinanceContext _context;

		public AccountsRepository(FinanceContext context)
		{
			_context = context;
		}

		public async Task<Account> GetAsync(int accountNumber)
		{
			var account = await _context.Accounts.FindAsync(accountNumber);
			if (account != null)
			{
				await _context.Entry(account)
					.Reference(a => a.CreditCard).LoadAsync();
				await _context.Entry(account)
					.Reference(a => a.Customer).LoadAsync();
				await _context.Entry(account)
					.Reference(a => a.AccountNumber).LoadAsync();
			}
			
			return account;
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
