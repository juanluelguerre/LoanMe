using LoanMe.Finance.Api.Application.Domain.Aggregates;
using LoanMe.Finance.Api.Application.Domain.Interfaces;
using LoanMe.Finance.Api.Application.Events;
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
		private readonly IDataService<Account> _dataService;

		public DepositAddCommandHandler(IMediator mediator, IDataService<Account> dataService, Logger<DepositAddCommandHandler> logger)
		{
			_logger = logger;			
			_mediator = mediator;
		}

		public async Task<bool> Handle(DepositAddCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Handle({nameof(AccountAddCommandHandler)}) -> {command}");

			var newAccount = command.Account;
			newAccount.AddAmount(command.Amount);
			var updated = _dataService.Update(newAccount);
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

			return new DepositAddedEvent(command.Account.IBAN);
		}
	}
}
