namespace LoanMe.Catalog.Api.Services.Events
{
	public class OrderStatusChangeToAwaitingValidationIntegrationEvent
	{
		public int OrderId { get; }
		public int ProductId { get; }
		public bool Available { get; }

		public OrderStatusChangeToAwaitingValidationIntegrationEvent(int orderId, int productId, bool available)
		{
			OrderId = orderId;
			ProductId = productId;
			Available = available;
		}
	}
}
