using DotNetCore.CAP;
using LoanMe.Catalog.Api.Application.Entities;
using LoanMe.Catalog.Api.IntegrationEvents.Events;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Threading.Tasks;

namespace LoanMe.Catalog.Api.IntegrationEvents.EventHandling
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
				var catalogItem = _catalogContext.CatalogItems.Find(@event.ProductId);				
				var productAvailable = catalogItem != null && catalogItem.AvailableStock > 0;

				if (productAvailable)
				{					
					await _eventBus.PublishAsync(nameof(OrderStockConfirmedEvent), new OrderStockConfirmedEvent(@event.OrderId));
				}
				else
				{
					await _eventBus.PublishAsync(nameof(OrderStockRejectedEvent), new OrderStockRejectedEvent(@event.OrderId));

				}
			}
		}
	}
}
