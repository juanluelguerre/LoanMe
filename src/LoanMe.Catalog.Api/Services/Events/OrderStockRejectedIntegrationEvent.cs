namespace LoanMe.Catalog.Api.Services.Events
{
	public class OrderStockRejectedIntegrationEvent
	{
		public int OrderId { get; }
		public OrderStockRejectedIntegrationEvent(int orderId) => OrderId = orderId;
	}
}
