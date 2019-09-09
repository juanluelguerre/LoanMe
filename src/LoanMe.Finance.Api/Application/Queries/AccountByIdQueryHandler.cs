using LoanMe.Finance.Api.Application.Domain.Aggregates.AccountAggregate;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace LoanMe.Finance.Api.Application.Queries
{
	public class AccountByIdQueryHandler : IRequestHandler<AccountByIdQuery, AccountViewModel>
	{
		private readonly ILogger _logger;
		private readonly IAccountsReadOnlyRepository _repository;

		public AccountByIdQueryHandler(IAccountsReadOnlyRepository repository, ILogger<AccountByIdQueryHandler> logger)
		{
			_logger = logger;
			_repository = repository;
		}

		public async Task<AccountViewModel> Handle(AccountByIdQuery query, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"{nameof(AccountAllQueryHandler)}.Handle({query})");
			
			var customer = await _repository.GetCustomerAsync(query.Id);
			return customer;
		}
	}
}

