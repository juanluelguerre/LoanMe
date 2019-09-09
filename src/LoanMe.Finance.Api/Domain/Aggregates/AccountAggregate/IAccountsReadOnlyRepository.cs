using LoanMe.Finance.Api.Application.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoanMe.Finance.Api.Application.Domain.Aggregates.AccountAggregate
{
	public interface IAccountsReadOnlyRepository
	{
		Task<AccountViewModel> GetCustomerAsync(int id);
		Task<IEnumerable<AccountViewModel>> GetCustomersAsync();
	}
}
