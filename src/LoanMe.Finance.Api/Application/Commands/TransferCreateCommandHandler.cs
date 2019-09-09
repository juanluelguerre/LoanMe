using LoanMe.Catalog.Api.Domain.Aggregates.AccountAggregate;
using LoanMe.Finance.Api.Doamin.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace LoanMe.Finance.Api.Application.Commands
{
	public class TransferCreateCommandHandler : IRequestHandler<TransferCreateCommand, bool>
	{
		private readonly ILogger _logger;
		private readonly IMediator _mediator;
		private readonly IAccountsRepository _repository;		

		public TransferCreateCommandHandler(IMediator mediator, IAccountsRepository repository, Logger<TransferCreateCommandHandler> logger)
		{
			_mediator = mediator;
			_repository = repository;
			_logger = logger;
		}

		public async Task<bool> Handle(TransferCreateCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Handle({nameof(DepositAddCommandHandler)}) -> {command}");

			var updatedSource = _repository.UpdateAmount(command.SourceAccount, command.Amount * -1);
			var updatedTarget = _repository.UpdateAmount(command.TargetAccount, command.Amount);
			if (updatedSource && updatedTarget)
			{
				await _mediator.Publish(Apply(command));
				return true;
			}
			return false;
		}

		private TransferCreatedEvent Apply(TransferCreateCommand command)
		{
			if (command == null)
			{
				throw new System.ArgumentNullException(nameof(command));
			}

			return new TransferCreatedEvent(command.SourceAccount.AccountNumber, command.TargetAccount.AccountNumber, command.Amount);
		}
	}
}
