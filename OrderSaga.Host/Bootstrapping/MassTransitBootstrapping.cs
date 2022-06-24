using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OrderSaga.Contracts;
using OrderSaga.Host.Consumers;
using OrderSaga.Host.Entities;
using OrderSaga.Host.StateMachines;

namespace OrderSaga.Host.Bootstrapping
{
    public static class MassTransitBootstrapping
    {
        public static IServiceCollection ConfigureMassTransit(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);

            services.AddMassTransit(busConfig =>
            {
                busConfig.AddConsumersFromNamespaceContaining<CreateOrderConsumer>();
                busConfig.AddConsumersFromNamespaceContaining<ChangeOrderStatusConsumer>();

                busConfig.AddSagaStateMachine<OrderStateMachine, Order>()
                    .NHibernateRepository();

                busConfig.UsingRabbitMq((context, busFactoryConfig) =>
                {
                    busFactoryConfig.Host(configuration["RabbitMq:Host"], "/", hostConfig =>
                    {
                        hostConfig.Username(configuration["RabbitMq:Credentials:Username"]);
                        hostConfig.Password(configuration["RabbitMq:Credentials:Password"]);
                    });

                    busFactoryConfig.ConfigureEndpoints(context);
                });
            });

            return services;
        }

    }
}
