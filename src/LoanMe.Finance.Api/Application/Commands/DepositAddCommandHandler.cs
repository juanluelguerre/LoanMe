using LoanMe.Catalog.Api.Domain.Aggregates.AccountAggregate;
using LoanMe.Finance.Api.Doamin.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace LoanMe.Finance.Api.Application.Commands
{
	public class DepositAddCommandHandler: IRequestHandler<DepositAddCommand, bool>
	{
		private readonly ILogger _logger;		
		private readonly IMediator _mediator;
		private readonly IAccountsRepository _customerRepository;

		public DepositAddCommandHandler(IMediator mediator, IAccountsRepository customerRepository, Logger<DepositAddCommandHandler> logger)
		{					
			_mediator = mediator;
			_customerRepository = customerRepository;
			_logger = logger;
		}

		public async Task<bool> Handle(DepositAddCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Handle({nameof(DepositAddCommandHandler)}) -> {command}");

			//var newAccount = command.Account.AccountNumber;
			//newAccount.AddAmount(command.Amount);

			var updated = _customerRepository.UpdateAmount(command.Account, command.Amount);
			if (updated)
			{
				await _mediator.Publish(Apply(command));
				return updated;
			}
			return false;
		}

		private DepositAddedEvent Apply(DepositAddCommand command)
		{
			if (command == null)
			{
				throw new System.ArgumentNullException(nameof(command));
			}

			return new DepositAddedEvent(command.Account.AccountNumber, command.Amount);
		}
	}
}
