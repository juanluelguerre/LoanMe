using LoanMe.ApplicationBlocks.EventBus.Abstractions;
using System.Threading.Tasks;

namespace LoanMe.ApplicationBlocks.Tests
{
	public class TestIntegrationEventHandler : IIntegrationEventHandler<TestIntegrationEvent>
    {
        public bool Handled { get; private set; }

        public TestIntegrationEventHandler()
        {
            Handled = false;
        }

        public async Task Handle(TestIntegrationEvent @event)
        {
            Handled = true;
        }
    }
}
