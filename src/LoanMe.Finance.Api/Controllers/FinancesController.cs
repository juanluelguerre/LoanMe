using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoanMe.Finance.Api.Controllers
{
	// [Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class BanksController : ControllerBase
	{
		private readonly IMediator _mediator;

		public BanksController(IMediator mediator)
		{
			_mediator = mediator;
		}

		//[HttpGet]
		////[Produces(typeof(IEnumerable<BankViewModel>))]
		//[ProducesResponseType(StatusCodes.Status400BadRequest)]
		//[ProducesResponseType(StatusCodes.Status404NotFound)]
		//[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		//public async Task<IActionResult> Get()
		//{
		//	var customers = await _mediator.Send(new CustomerAllQuery());			
		//	return Ok(customers);
		//}
		
		
	}
}
