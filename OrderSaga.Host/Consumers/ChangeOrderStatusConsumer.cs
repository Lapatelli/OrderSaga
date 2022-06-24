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
            _logger.LogInformation($"ChangeOrderStatus: Order {context.Message.OrderNumber} to state: {context.Message.Status}");
            
            var orderStatusChangedMessage = CreateOrderStatusChangedMessage(context.Message);
            await context.Publish(orderStatusChangedMessage);

            await context.RespondAsync(CreateStatusChangingAcceptedMessage(context.Message));
        }

        private static OrderStatusChanged CreateOrderStatusChangedMessage(ChangeOrderStatus message) =>
            new OrderStatusChanged(message.OrderNumber, message.Status, message.UpdatedDate);

        private static OrderStatusChangingAccepted CreateStatusChangingAcceptedMessage(ChangeOrderStatus message) =>
            new OrderStatusChangingAccepted(message.OrderNumber, message.Status, message.UpdatedDate);
    }
}
