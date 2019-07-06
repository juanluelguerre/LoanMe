using AutoMapper;
using LoanMe.Catalog.Api.Application.Entities;

namespace LoanMe.Catalog.Api.Application.Models
{
	public class CustomerProfile : Profile
	{
		public CustomerProfile()
		{
			CreateMap<CustomerModel, Customer>();
			CreateMap<Customer, CustomerModel>();				
		}
	}
}
