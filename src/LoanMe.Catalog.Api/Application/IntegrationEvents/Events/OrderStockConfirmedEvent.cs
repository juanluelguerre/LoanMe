namespace LoanMe.Catalog.Api.Application.IntegrationEvents.Events
{
	public class OrderStockConfirmedEvent
	{
		public int OrderId { get; }
		public OrderStockConfirmedEvent(int orderId) => OrderId = orderId;
	}
}
