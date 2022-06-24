using MassTransit;
using Microsoft.Extensions.Logging;
using OrderSaga.Contracts;
using System;
using System.Threading.Tasks;

namespace OrderSaga.Host.Consumers
{
    public class CreateOrderConsumer : IConsumer<CreateOrder>
    {
        private readonly ILogger<CreateOrderConsumer> _logger;

        public CreateOrderConsumer(ILogger<CreateOrderConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CreateOrder> context)
        {
            var orderId = Guid.NewGuid();

            _logger.LogInformation($"CreateOrderConsumer: proccess orderId: {orderId}.");

            if (string.IsNullOrEmpty(context.Message.CustomerName))
            {
                await context.RespondAsync(CreateOrderCreationRejectedMessage(context.Message, orderId));
                return;
            }

            var orderCreatedMessage = CreateOrderCreatedMessage(context.Message, orderId);
            await context.Publish(orderCreatedMessage);

            await context.RespondAsync(CreateOrderCreationAcceptedMessage(orderCreatedMessage));
        }

        private static OrderCreated CreateOrderCreatedMessage(
            CreateOrder message,
            Guid orderId)
        {
            var orderNumber = (orderId.GetHashCode() & 0x7fffffff) % 100000000;

            return new OrderCreated(
                orderId,
                orderNumber,
                message.OrderDate,
                message.CustomerName,
                message.CustomerSurname,
                message.Items);
        }

        private static OrderCreationAccepted CreateOrderCreationAcceptedMessage(OrderCreated message) =>
            new OrderCreationAccepted(
                message.OrderId,
                message.OrderDate,
                message.OrderNumber,
                message.CustomerName,
                message.CustomerSurname,
                message.Items);

        private static OrderCreationRejected CreateOrderCreationRejectedMessage(CreateOrder message, Guid orderId) =>
            new OrderCreationRejected(
                orderId,
                message.OrderDate,
                $"Please specify customer name");
    }
}
