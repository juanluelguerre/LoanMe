using MediatR;
using Microsoft.Extensions.Logging;
using LoanMe.Finance.Api.Application.Domain.Aggregates;
using LoanMe.Finance.Api.Application.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LoanMe.Finance.Api.Application.Domain.Interfaces;

namespace LoanMe.Finance.Api.Application.Commands
{
	public class AccountAddCommandHandler: IRequestHandler<AccountAddCommand, bool>
	{
		private readonly ILogger _logger;		
		private readonly IMediator _mediator;
		private readonly IDataService<Account> _dataService;

		public AccountAddCommandHandler(IMediator mediator, IDataService<Account> dataService, Logger<AccountAddCommandHandler> logger)
		{
			_logger = logger;			
			_mediator = mediator;
		}

		public async Task<bool> Handle(AccountAddCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Handle({nameof(AccountAddCommandHandler)}) -> {command}");			

			var account = new Account(command.Entity, command.Office, command.Control, command.Number);

			var result = _dataService.Add(account);

			await _mediator.Publish(Apply(command));

			return result;
		}

		private AccountAddedEvent Apply(AccountAddCommand command)
		{
			if (command == null)
			{
				throw new System.ArgumentNullException(nameof(command));
			}

			return new AccountAddedEvent(command.IBAN);
		}
	}
}
