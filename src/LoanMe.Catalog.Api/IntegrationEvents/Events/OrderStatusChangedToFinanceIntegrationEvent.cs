namespace LoanMe.Catalog.Api.IntegrationEvents.Events
{
	public class OrderStatusChangedToFinanceIntegrationEvent
	{
		public int OrderId { get; set; }
		public int ProductId { get; set; }

		// Just one unit per Order. So One order means one Product to Finance it !
		public int Units { get; } = 1; 
	}
}
