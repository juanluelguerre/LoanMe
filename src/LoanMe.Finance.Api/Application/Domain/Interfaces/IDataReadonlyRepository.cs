using LoanMe.Customers.Api.Application.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoanMe.Finance.Api.Application.Domain.Interfaces
{
	public interface IDataReadonlyRepository
	{
		Task<CustomerViewModel> FindOne(int id);				
		Task<IEnumerable<CustomerViewModel>> FindAll();		
	}
}