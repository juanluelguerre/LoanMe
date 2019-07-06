using LoanMe.Catalog.Api.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoanMe.Catalog.Api.Application.Repository
{
	public interface ICustomerRepository
	{
		Task<IEnumerable<CustomerModel>> GetAll();
		Task<CustomerModel> GetById(int id);		
		Task<bool> Add(CustomerModel customer);
		Task<bool> Update(int id, CustomerModel customer);
		Task<bool> Delete(int id);
	}
}
