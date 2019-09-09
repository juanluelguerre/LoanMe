using LoanMe.Finance.Api.Domain.Aggregates;
using MediatR;

namespace LoanMe.Finance.Api.Application.Commands
{
	public class TransferCreateCommand : IRequest<bool>
	{
		public Account SourceAccount { get; set; }
		public Account TargetAccount { get; set; }
		public decimal Amount { get; set; }
	}
}
