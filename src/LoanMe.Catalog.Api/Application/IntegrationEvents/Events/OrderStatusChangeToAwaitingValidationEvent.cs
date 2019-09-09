namespace LoanMe.Catalog.Api.Application.IntegrationEvents.Events
{
	public class OrderStatusChangeToAwaitingValidationEvent
	{
		public int OrderId { get; }
		public int ProductId { get; }
		public bool Available { get; }

		public OrderStatusChangeToAwaitingValidationEvent(int orderId, int productId, bool available)
		{
			OrderId = orderId;
			ProductId = productId;
			Available = available;
		}
	}
}
