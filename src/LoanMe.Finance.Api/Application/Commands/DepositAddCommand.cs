using LoanMe.Finance.Api.Application.Domain.Aggregates;
using MediatR;

namespace LoanMe.Finance.Api.Application.Commands
{
	public class DepositAddCommand : IRequest<bool>
	{
		public Account Account { get; set; }
		public double Amount { get; set; }
	}
}
