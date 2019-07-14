using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanMe.Finance.Api.Application.Commands
{
	public class CancelDepositCommand : IRequest<bool>
	{
		public int OrderNumber { get; private set; }

		public CancelDepositCommand(int orderNumber)
		{
			OrderNumber = orderNumber;
		}
	}
}
