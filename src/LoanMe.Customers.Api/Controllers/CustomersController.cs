using LoanMe.Customers.Api.Application.Models;
using LoanMe.Customers.Api.Application.Queries;
using LoanMe.Customers.Api.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoanMe.Customers.Api.Controllers
{
	// [Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class CustomersController : ControllerBase
	{
		private readonly ICustomerService _service;

		public CustomersController(ICustomerService service)
		{
			_service = service;
		}

		[HttpGet]
		[Produces(typeof(IEnumerable<CustomerViewModel>))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Get()
		{
			var customers = await _service.GetAll();
			if (customers == null)
				return BadRequest(customers);

			return Ok(customers);
		}

		[HttpGet("{id}")]
		[Produces(typeof(CustomerViewModel))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Get(int id)
		{
			var customer = await _service.GetById(id);			
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
		public async Task<IActionResult> Post([FromBody] CustomerRequest customer)
		{
			var saved = await _service.Add(customer);
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
		public async Task<IActionResult> Put(int id, [FromBody] CustomerRequest customer)
		{
			var saved = await _service.Update(id, customer);
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
			var deleted = await _service.Delete(id);
			if (!deleted)
				return BadRequest(deleted);

			return Ok(deleted);
		}
	}
}
