namespace LoanMe.Catalog.Api.Application.IntegrationEvents.Events
{
	public class ProductPriceChangedEvent
	{
		public int ProductId { get; private set; }
		public decimal NewPrice { get; private set; }
		public decimal OldPrice { get; private set; }

		public ProductPriceChangedEvent(int productId, decimal newPrice, decimal oldPrice)
		{
			ProductId = productId;
			NewPrice = newPrice;
			OldPrice = oldPrice;
		}
	}
}
