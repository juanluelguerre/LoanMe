using System.Collections.Generic;

namespace LoanMe.Catalog.Api.Services.Events
{
	public class OrderStockRejectedEvent
	{
		public int OrderId { get; }

		public List<ConfirmedOrderStockItem> OrderStockItems { get; }

		public OrderStockRejectedEvent(int orderId,
			List<ConfirmedOrderStockItem> orderStockItems)
		{
			OrderId = orderId;
			OrderStockItems = orderStockItems;
		}
	}

	public class ConfirmedOrderStockItem
	{
		public int ProductId { get; }
		public bool HasStock { get; }

		public ConfirmedOrderStockItem(int productId, bool hasStock)
		{
			ProductId = productId;
			HasStock = hasStock;
		}
	}
}
