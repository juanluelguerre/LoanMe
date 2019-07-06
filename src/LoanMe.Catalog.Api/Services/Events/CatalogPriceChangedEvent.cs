namespace LoanMe.Catalog.Api.Services.Events
{
	public class CatalogPriceChangedEvent
	{
		public int ProductId { get; private set; }
		public decimal NewPrice { get; private set; }
		public decimal OldPrice { get; private set; }

		public CatalogPriceChangedEvent(int productId, decimal newPrice, decimal oldPrice)
		{
			ProductId = productId;
			NewPrice = newPrice;
			OldPrice = oldPrice;
		}
	}
}
