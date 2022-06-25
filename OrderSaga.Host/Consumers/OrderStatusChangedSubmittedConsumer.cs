using MassTransit;
using OrderSaga.Contracts;
using System.Threading.Tasks;

namespace OrderSaga.Host.Consumers
{
    public class OrderStatusChangedSubmittedConsumer : IConsumer<OrderStatusChangedSubmitted>
    {
        public Task Consume(ConsumeContext<OrderStatusChangedSubmitted> context)
        {
            // Some logic for consumption can be implemented.

            return Task.CompletedTask;
        }
    }
}