using LoanMe.Finance.Api.Domain.Aggregates;
using MediatR;

namespace LoanMe.Finance.Api.Application.Commands
{
	public class DepositAddCommand : IRequest<bool>
	{
		public Account Account { get; set; }
		public decimal Amount { get; set; }
	}
}
