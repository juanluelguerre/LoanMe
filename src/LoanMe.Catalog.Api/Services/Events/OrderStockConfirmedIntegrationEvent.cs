namespace LoanMe.Catalog.Api.Services.Events
{
	public class OrderStockConfirmedIntegrationEvent
	{
		public int OrderId { get; }
		public OrderStockConfirmedIntegrationEvent(int orderId) => OrderId = orderId;
	}
}
