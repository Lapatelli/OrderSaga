using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

        public Task ConsumeFault<T>(ConsumeContext<T> context, TimeSpan duration, string consumerType, Exception exception) where T : class
        {
            _logger.LogError(exception, exception.Message);
            return Task.CompletedTask;
        }

        public Task PostConsume<T>(ConsumeContext<T> context, TimeSpan duration, string consumerType) where T : class
        {
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

        public Task ReceiveFault(ReceiveContext context, Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            return Task.CompletedTask;
        }
    }
}
