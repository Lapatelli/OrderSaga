using MassTransit;
using Microsoft.Extensions.Logging;
using OrderSaga.Contracts;
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
            _logger.LogInformation($"CreateOrderConsumer: {context.Message.OrderNumber}");

            if (context.Message.OrderNumber == 1)
            {
                await context.RespondAsync(new OrderCreationRejected(
                    context.Message.OrderId,
                    context.Message.OrderDate,
                    context.Message.OrderNumber,
                    $"Invalid ordernumber"));

                return;
            }

            var orderCreatedMessage = new OrderCreated(
                context.Message.OrderId,
                context.Message.OrderNumber,
                context.Message.OrderDate,
                context.Message.CustomerName,
                context.Message.CustomerSurname,
                context.Message.Items);

            await context.Publish(orderCreatedMessage);

            await context.RespondAsync(new OrderCreationAccepted(
                context.Message.OrderId,
                InVar.Timestamp,
                context.Message.OrderNumber,
                context.Message.CustomerName,
                context.Message.CustomerSurname,
                context.Message.Items));
        }
    }
}
