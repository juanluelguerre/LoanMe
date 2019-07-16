using DotNetCore.CAP;
using LoanMe.Catalog.Api.Application.Entities;
using LoanMe.Catalog.Api.IntegrationEvents.Events;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Threading.Tasks;

namespace LoanMe.Catalog.Api.IntegrationEvents.EventHandling
{
	public class ProductPriceChangeEventHandler : ICapSubscribe
	{
		private readonly CatalogContext _catalogContext;
		private readonly ILogger<ProductPriceChangeEventHandler> _logger;

		public ProductPriceChangeEventHandler(
			CatalogContext catalogContext,
			ILogger<ProductPriceChangeEventHandler> logger)
		{
			_catalogContext = catalogContext;
			_logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
		}

		[CapSubscribe(nameof(ProductPriceChangedEvent))]
		public async Task Handle(ProductPriceChangedEvent @event)
		{
			using (LogContext.PushProperty("IntegrationEventContext", $"{@event.ProductId}-{Program.AppName}"))
			{
				_logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.ProductId, Program.AppName, @event);

				// TODO: To be completed ...
				// @event.

				await _catalogContext.SaveChangesAsync();
			}
		}
	}
}
