using System.Threading.Tasks;

namespace LoanMe.ApplicationBlocks.EventBus.Abstractions
{
	public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
