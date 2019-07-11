using LoanMe.ApplicationBlocks.EventBus.Events;
using System.Threading.Tasks;

namespace LoanMe.Catalog.Api.Services
{
	public interface ICatalogEventService
	{
		Task SaveEventAndCatalogContextChangesAsync(IntegrationEvent evt);
		Task PublishThroughEventBusAsync(IntegrationEvent evt);
	}
}
