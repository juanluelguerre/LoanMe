using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanMe.Finance.Api.Application.Domain.Aggregates
{
	public class AccountOperation : AggregateRoot
	{
		public string AccountId { get; set; }
		public int OperationId { get; set; }
	}
}
