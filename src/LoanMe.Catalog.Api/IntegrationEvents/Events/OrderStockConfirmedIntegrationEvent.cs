namespace LoanMe.Catalog.Api.IntegrationEvents.Events
{
	public class OrderStockConfirmedIntegrationEvent
	{
		public int OrderId { get; }
		public OrderStockConfirmedIntegrationEvent(int orderId) => OrderId = orderId;
	}
}
