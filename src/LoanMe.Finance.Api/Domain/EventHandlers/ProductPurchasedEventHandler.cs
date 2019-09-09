using System.Threading;
using System.Threading.Tasks;
using DotNetCore.CAP;
using LoanMe.Finance.Api.Application.Infrastructure.IntegrationEvents.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LoanMe.Finance.Api.Domain.EventHanlers.Events
{
	public class ProductPurchasedEventHandler : INotificationHandler<ProductPurchasedEvent>
	{
		private readonly ICapPublisher _eventBus;
		private readonly ILogger _logger;

		public ProductPurchasedEventHandler(ICapPublisher eventBus, ILogger<ProductPurchasedEventHandler> logger)
		{
			_eventBus = eventBus;
			_logger = logger;
		}

		public Task Handle(ProductPurchasedEvent notification, CancellationToken cancellationToken)
		{
			//
			// TODO: Publish vía _eventbus and subscribe Catalog, to remove product quantity !
			//


			throw new System.NotImplementedException();
		}
	}
}
