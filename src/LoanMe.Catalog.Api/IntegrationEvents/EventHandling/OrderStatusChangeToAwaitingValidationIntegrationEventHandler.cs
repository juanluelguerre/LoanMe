using DotNetCore.CAP;
using LoanMe.Catalog.Api.Application.Entities;
using LoanMe.Catalog.Api.IntegrationEvents.Events;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Threading.Tasks;

namespace LoanMe.Catalog.Api.IntegrationEvents.EventHandling
{
	public class OrderStatusChangeToAwaitingValidationIntegrationEventHandler : ICapSubscribe
	{
		private readonly CatalogContext _catalogContext;
		private readonly ICapPublisher _eventBus;
		private readonly ILogger<OrderStatusChangeToAwaitingValidationIntegrationEventHandler> _logger;

		public OrderStatusChangeToAwaitingValidationIntegrationEventHandler(
			CatalogContext catalogContext,
			ICapPublisher eventBus,
			ILogger<OrderStatusChangeToAwaitingValidationIntegrationEventHandler> logger)
		{
			_catalogContext = catalogContext;
			_eventBus = eventBus;
			_logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
		}

		[CapSubscribe(nameof(OrderStatusChangeToAwaitingValidationIntegrationEvent))]
		public async Task Handle(OrderStatusChangeToAwaitingValidationIntegrationEvent @event)
		{
			using (LogContext.PushProperty("IntegrationEventContext", $"{Program.AppName}"))
			{
				var catalogItem = _catalogContext.CatalogItems.Find(@event.ProductId);				
				var productAvailable = catalogItem != null && catalogItem.AvailableStock > 0;

				if (productAvailable)
				{					
					await _eventBus.PublishAsync(nameof(OrderStockConfirmedIntegrationEvent), new OrderStockConfirmedIntegrationEvent(@event.OrderId));
				}
				else
				{
					await _eventBus.PublishAsync(nameof(OrderStockRejectedIntegrationEvent), new OrderStockRejectedIntegrationEvent(@event.OrderId));

				}
			}
		}
	}
}
