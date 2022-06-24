using MassTransit;
using OrderSaga.Contracts;
using System.Threading.Tasks;

namespace OrderSaga.Host.Consumers
{
    public class ChangeOrderStatusConsumer : IConsumer<ChangeOrderStatus>
    {
        public async Task Consume(ConsumeContext<ChangeOrderStatus> context)
        {
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
