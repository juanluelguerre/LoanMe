namespace LoanMe.Catalog.Api.Application.IntegrationEvents.Events
{
	public class OrderStockRejectedEvent
	{
		public int OrderId { get; }
		public OrderStockRejectedEvent(int orderId) => OrderId = orderId;
	}
}
