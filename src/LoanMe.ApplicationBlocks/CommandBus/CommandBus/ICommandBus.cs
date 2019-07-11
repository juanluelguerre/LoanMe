using LoanMe.ApplicationBlocks.EventBus.CommandBus;
using System.Threading.Tasks;

namespace LoanMe.ApplicationBlocks.CommandBus
{
	public interface ICommandBus
    {
        Task SendAsync<T>(T command) where T : IntegrationCommand;

    }
}
