using DotNetCore.CAP;
using LoanMe.Catalog.Api.Application.Entities;
using LoanMe.Catalog.Api.Services.Events;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanMe.Catalog.Api.Services.EventHandling
{
	public class OrderStatusChangeToAwaitingValidationEventHandler : ICapSubscribe
	{
		private readonly CatalogContext _catalogContext;
		private readonly ICapPublisher _eventBus;
		private readonly ILogger<OrderStatusChangeToAwaitingValidationEventHandler> _logger;

		public OrderStatusChangeToAwaitingValidationEventHandler(
			CatalogContext catalogContext,
			ICapPublisher eventBus,
			ILogger<OrderStatusChangeToAwaitingValidationEventHandler> logger)
		{
			_catalogContext = catalogContext;
			_eventBus = eventBus;
			_logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
		}

		[CapSubscribe(nameof(OrderStatusChangeToAwaitingValidationEvent))]
		public async Task Handle(OrderStatusChangeToAwaitingValidationEvent @event)
		{
			using (LogContext.PushProperty("IntegrationEventContext", $"{Program.AppName}"))
			{
				var confirmedOrderStockItems = new List<ConfirmedOrderStockItem>();

				foreach (var orderStockItem in @event.OrderStockItems)
				{
					var catalogItem = _catalogContext.CatalogItems.Find(orderStockItem.ProductId);
					var hasStock = catalogItem.AvailableStock >= orderStockItem.Units;
					var confirmedOrderStockItem = new ConfirmedOrderStockItem(catalogItem.Id, hasStock);

					confirmedOrderStockItems.Add(confirmedOrderStockItem);
				}

				if (confirmedOrderStockItems.Any(c => !c.HasStock))
				{
					await _eventBus.PublishAsync(nameof(OrderStockRejectedEvent),
						new OrderStockRejectedEvent(@event.OrderId, confirmedOrderStockItems));
				}
				else
				{
					await _eventBus.PublishAsync(nameof(OrderStockConfirmedEvent),
						new OrderStockConfirmedEvent(@event.OrderId));
				}
			}
		}
	}
}
