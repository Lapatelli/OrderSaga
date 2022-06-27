using MassTransit;
using Microsoft.Extensions.Logging;
using OrderSaga.Contracts;
using System;
using System.Text;
using System.Threading.Tasks;

namespace OrderSaga.Host.Observers
{
    public class ReceiveObserver : IReceiveObserver
    {
        private readonly ILogger<ReceiveObserver> _logger;

        public ReceiveObserver(ILogger<ReceiveObserver> logger)
        {
            _logger = logger;
        }

        public Task PostConsume<T>(
            ConsumeContext<T> context,
            TimeSpan duration,
            string consumerType) where T : class
        {
            if (context.TryGetMessage<OrderStatusChangedRejected>(out var statusChangedRejectedContext))
            {
                _logger.LogInformation(GetRejectedMessageToLog(statusChangedRejectedContext));
            }

            if (context.TryGetMessage<OrderStatusChangedSubmitted>(out var statusChangedSubmittedContext))
            {
                _logger.LogInformation(GetSubmittedMessageToLog(statusChangedSubmittedContext));
            }

            if (context.TryGetMessage<OrderCreated>(out var orderCreatedContext))
            {
                _logger.LogInformation(GetOrderCreatedMessageToLog(orderCreatedContext));
            }

            return Task.CompletedTask;
        }

        public Task ConsumeFault<T>(
            ConsumeContext<T> context,
            TimeSpan duration,
            string consumerType,
            Exception exception) where T : class
        {
            _logger.LogError(exception, exception.Message);
            return Task.CompletedTask;
        }

        public Task ReceiveFault(
            ReceiveContext context,
            Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            return Task.CompletedTask;
        }

        public Task PostReceive(ReceiveContext context)
        {
            return Task.CompletedTask;
        }

        public Task PreReceive(ReceiveContext context)
        {
            return Task.CompletedTask;
        }

        private static string GetRejectedMessageToLog(ConsumeContext<OrderStatusChangedRejected> context)
        {
            var message = context.Message;
            return $"The order: {message.OrderNumber} " +
                $"can not be transitioned from '{message.CurrentOrderStatus}' " +
                $"state into '{message.IntendedOrderStatus}' state.";
        }

        private static string GetSubmittedMessageToLog(ConsumeContext<OrderStatusChangedSubmitted> context)
        {
            var message = context.Message;
            return $"The order: {message.OrderNumber} has been transitioned to '{message.CurrentOrderStatus}' state.";
        }

        private static string GetOrderCreatedMessageToLog(ConsumeContext<OrderCreated> context)
        {
            var message = context.Message;
            return new StringBuilder()
                .Append("The order has been created:")
                .Append($"orderId = {message.OrderId}; ")
                .Append($"orderNumber = {message.OrderNumber}; ")
                .Append($"customerName = '{message.CustomerName}'; ")
                .Append($"customerSurname = '{message.CustomerSurname}'; ")
                .Append($"orderDate = {message.OrderDate}; ")
                .ToString();
        }
    }
}
