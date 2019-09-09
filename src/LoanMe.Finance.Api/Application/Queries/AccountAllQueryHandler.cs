using LoanMe.Finance.Api.Application.Domain.Aggregates.AccountAggregate;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LoanMe.Finance.Api.Application.Queries
{
	public class AccountAllQueryHandler : IRequestHandler<AccountAllQuery, IEnumerable<AccountViewModel>>
	{
		private readonly ILogger _logger;
		private readonly IAccountsReadOnlyRepository _repository;

		public AccountAllQueryHandler(IAccountsReadOnlyRepository repository, ILogger<AccountAllQueryHandler> logger)
		{
			_logger = logger;
			_repository = repository;
		}

		public async Task<IEnumerable<AccountViewModel>> Handle(AccountAllQuery query, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"{nameof(AccountAllQueryHandler)}.Handle({query})");
			// var customers = _repository.FindAll("SELECT Id, FirstName, LastName FROM Customers").Result;
			var customers = await _repository.GetCustomersAsync();

			// TODO: Update AccountViewModel to make a sample with just some accounts values, but no all of them.
			var list = customers.Select(c => new AccountViewModel(c.Account));

			return list;
		}
	}
}
