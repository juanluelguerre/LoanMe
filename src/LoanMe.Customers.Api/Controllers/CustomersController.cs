using LoanMe.Catalog.Api.Application.Models;
using LoanMe.Catalog.Api.Application.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoanMe.Catalog.Api.Controllers
{
	// [Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class CustomersController : ControllerBase
	{
		private readonly ICustomerRepository _repository;

		public CustomersController(ICustomerRepository repository)
		{
			_repository = repository;
		}

		[HttpGet]
		[Produces(typeof(IEnumerable<CustomerModel>))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Get()
		{
			var customers = await _repository.GetAll();
			if (customers == null)
				return BadRequest(customers);

			return Ok(customers);
		}

		[HttpGet("{id}")]
		[Produces(typeof(CustomerModel))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Get(int id)
		{
			var customer = await _repository.GetById(id);			
			if (customer == null)
				return BadRequest(customer);

			return Ok(customer);
		}

		[HttpPost]
		[Produces(typeof(bool))]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Post([FromBody] CustomerModel customer)
		{
			var saved = await _repository.Add(customer);
			if (!saved)
				return BadRequest(saved);

			return Ok(saved);
		}

		[HttpPut("{id}")]
		[Produces(typeof(bool))]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Put(int id, [FromBody] CustomerModel customer)
		{
			var saved = await _repository.Update(id, customer);
			if (!saved)
			{
				ModelState.AddModelError("Customer.Id", $"Cutomer.Id '{customer.Id}' and id '{id}' must be the same !");
				return BadRequest(ModelState);
			}

			return Ok(saved);				
		}

		[HttpDelete("{id}")]
		[Produces(typeof(bool))]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Delete(int id)
		{
			var deleted = await _repository.Delete(id);
			if (!deleted)
				return BadRequest(deleted);

			return Ok(deleted);
		}
	}
}
