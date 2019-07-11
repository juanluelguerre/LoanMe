using LoanMe.ApplicationBlocks.EventBus.Abstractions;
using LoanMe.Catalog.Api.Application.Entities;
using LoanMe.Catalog.Api.Services.Events;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Threading.Tasks;

namespace LoanMe.Catalog.Api.Services.EventHandling
{
	public class CatalogPriceChangeEventHandler : IIntegrationEventHandler<ProductPriceChangedEvent>
	{
		private readonly CatalogContext _catalogContext;
		private readonly ILogger<CatalogPriceChangeEventHandler> _logger;

		public CatalogPriceChangeEventHandler(
			CatalogContext catalogContext,
			ILogger<CatalogPriceChangeEventHandler> logger)
		{
			_catalogContext = catalogContext;
			_logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
		}

		public async Task Handle(ProductPriceChangedEvent @event)
		{
			using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
			{
				_logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

				// TODO: To be completed ...
				// @event.

				await _catalogContext.SaveChangesAsync();
			}
		}
	}
}
