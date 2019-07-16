namespace LoanMe.Catalog.Api.IntegrationEvents.Events
{
	public class OrderStockConfirmedEvent
	{
		public int OrderId { get; }
		public OrderStockConfirmedEvent(int orderId) => OrderId = orderId;
	}
}
