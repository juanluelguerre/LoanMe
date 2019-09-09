using MediatR;
using System.Collections.Generic;

namespace LoanMe.Finance.Api.Application.Queries
{
	public class AccountAllQuery : IRequest<IEnumerable<AccountViewModel>>
	{

	}
}
