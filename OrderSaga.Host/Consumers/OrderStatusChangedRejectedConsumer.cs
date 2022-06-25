using MassTransit;
using OrderSaga.Contracts;
using System.Threading.Tasks;

namespace OrderSaga.Host.Consumers
{
    public class OrderStatusChangedRejectedConsumer : IConsumer<OrderStatusChangedRejected>
    {
        public Task Consume(ConsumeContext<OrderStatusChangedRejected> context)
        {
            // The logic for message consumption can be implemented.

            return Task.CompletedTask;
        }
    }
}
