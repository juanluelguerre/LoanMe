using LoanMe.Finance.Api.Application.Commands;
using LoanMe.Finance.Api.Application.Domain.Aggregates.AccountAggregate;
using LoanMe.Finance.Api.Domain.Aggregates.CustomerAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LoanMe.Finance.Api.Controllers
{
	// [Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class AccountsController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly IAccountsReadOnlyRepository _customerQueries;
		private readonly ILogger _logger;

		public AccountsController(IMediator mediator, IAccountsReadOnlyRepository customersQueries, ILogger<AccountsController> logger)
		{
			_mediator = mediator;
			_customerQueries = customersQueries;
			_logger = logger;
		}

		[HttpGet]
		[Produces(typeof(Customer))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetCustomerAsync(int id)
		{
			// TODO: Update using mediator and Commands
			// await _mediator.Send(command);

			var customers = await _customerQueries.GetCustomerAsync(id);
			return Ok(customers);
		}

		[HttpPut]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> AddDeposit([FromBody]DepositAddCommand command)
		{
			_logger.LogInformation($"Sending Command: {nameof(command)} ({command.Account.AccountNumber} - {command.Amount})");

			var customers = await _mediator.Send(command);
			return Ok(customers);
		}
	}
}
