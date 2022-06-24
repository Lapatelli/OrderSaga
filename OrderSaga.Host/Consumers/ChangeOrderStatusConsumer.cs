using MassTransit;
using Microsoft.Extensions.Logging;
using OrderSaga.Contracts;
using System.Threading.Tasks;

namespace OrderSaga.Host.Consumers
{
    public class ChangeOrderStatusConsumer : IConsumer<ChangeOrderStatus>
    {
        private readonly ILogger<ChangeOrderStatusConsumer> _logger;

        public ChangeOrderStatusConsumer(ILogger<ChangeOrderStatusConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ChangeOrderStatus> context)
        {
            _logger.LogInformation($"ChangeOrderStatus: {context.Message.OrderId} -- {context.Message.Status}");

            await context.Publish(
                new OrderStatusChanged(
                    context.Message.OrderId,
                    context.Message.Status));

            await context.RespondAsync(
                new OrderStatusChangingAccepted(
                    context.Message.OrderId,
                    context.Message.Status,
                    InVar.Timestamp));
        }
    }
}
