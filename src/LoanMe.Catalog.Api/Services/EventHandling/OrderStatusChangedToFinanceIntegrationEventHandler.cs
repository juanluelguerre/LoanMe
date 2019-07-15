using DotNetCore.CAP;
using LoanMe.Catalog.Api.Application.Entities;
using LoanMe.Catalog.Api.Services.Events;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Threading.Tasks;

namespace LoanMe.Catalog.Api.Services.EventHandling
{
	public class OrderStatusChangedToFinanceIntegrationEventHandler : ICapSubscribe
	{
		private readonly CatalogContext _catalogContext;
		private readonly ILogger<OrderStatusChangedToFinanceIntegrationEventHandler> _logger;

		public OrderStatusChangedToFinanceIntegrationEventHandler(
			CatalogContext catalogContext,
			ILogger<OrderStatusChangedToFinanceIntegrationEventHandler> logger)
		{
			_catalogContext = catalogContext;
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		[CapSubscribe(nameof(OrderStatusChangedToFinanceIntegrationEvent))]
		public async Task Handle(OrderStatusChangedToFinanceIntegrationEvent @event)
		{
			using (LogContext.PushProperty("IntegrationEventContext", $"{Program.AppName}"))
			{
				_logger.LogInformation("----- Handling integration event: {AppName} - ({@IntegrationEvent})", Program.AppName, @event);

				//we're not blocking stock/inventory
				var catalogItem = _catalogContext.CatalogItems.Find(@event.ProductId);

				catalogItem.RemoveStock(@event.Units);

				await _catalogContext.SaveChangesAsync();
			}
		}
	}
}
