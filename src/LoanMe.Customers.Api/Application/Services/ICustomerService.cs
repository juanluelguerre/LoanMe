using LoanMe.Customers.Api.Application.Models;
using LoanMe.Customers.Api.Application.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoanMe.Customers.Api.Application.Services
{
	public interface ICustomerService
	{
		Task<IEnumerable<CustomerViewModel>> GetAll();
		Task<CustomerViewModel> GetById(int id);		
		Task<bool> Add(CustomerRequest customer);
		Task<bool> Update(int id, CustomerRequest customer);
		Task<bool> Delete(int id);
	}
}
