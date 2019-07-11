using AutoMapper;
using LoanMe.Catalog.Api.Application.Entities;
using LoanMe.Catalog.Api.Application.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanMe.Catalog.Api.Application.Repository
{
	public class CustomerRepository : ICustomerRepository
	{
		private DataContext _context;
		private IMapper _mapper;

		public CustomerRepository(DataContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<IEnumerable<CustomerModel>> GetAll()
		{
			var customers = await _context.Customers.ToListAsync(); ;
			return _mapper.Map<List<CustomerModel>>(customers);
		}

		public async Task<CustomerModel> GetById(int id)
		{
			var customer = await _context.Customers
				.FirstOrDefaultAsync(c => c.Id == id);				
			return _mapper.Map<CustomerModel>(customer);
		}

		public Task<bool> Update(int id, CustomerModel customer)
		{
			throw new NotImplementedException();
		}

		public Task<bool> Add(CustomerModel customer)
		{
			throw new NotImplementedException();
		}

		public Task<bool> Delete(int id)
		{
			throw new NotImplementedException();
		}
	}
}
