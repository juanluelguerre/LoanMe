using DotNetCore.CAP;
using LoanMe.Catalog.Api.Application.Entities;
using LoanMe.Catalog.Api.Application.IntegrationEvents.Events;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Threading.Tasks;

namespace LoanMe.Catalog.Api.Application.IntegrationEvents.EventHandlers
{
	public class OrderStatusChangedToFinanceEventHandler : ICapSubscribe
	{
		private readonly CatalogContext _catalogContext;
		private readonly ILogger<OrderStatusChangedToFinanceEventHandler> _logger;

		public OrderStatusChangedToFinanceEventHandler(
			CatalogContext catalogContext,
			ILogger<OrderStatusChangedToFinanceEventHandler> logger)
		{
			_catalogContext = catalogContext;
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		[CapSubscribe(nameof(OrderStatusChangedToFinanceEvent))]
		public async Task Handle(OrderStatusChangedToFinanceEvent @event)
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
