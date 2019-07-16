namespace LoanMe.Catalog.Api.IntegrationEvents.Events
{
	public class OrderStockRejectedEvent
	{
		public int OrderId { get; }
		public OrderStockRejectedEvent(int orderId) => OrderId = orderId;
	}
}
