using MediatR;

namespace LoanMe.Finance.Api.Application.Queries
{
	public class AccountByIdQuery : IRequest<AccountViewModel>
	{
		public int Id { get; set; }
	}
}
