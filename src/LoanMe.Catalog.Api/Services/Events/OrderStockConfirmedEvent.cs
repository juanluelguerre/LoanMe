using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanMe.Catalog.Api.Services.Events
{
	public class OrderStockConfirmedEvent
	{
		public int OrderId { get; }
		public OrderStockConfirmedEvent(int orderId) => OrderId = orderId;
	}
}
