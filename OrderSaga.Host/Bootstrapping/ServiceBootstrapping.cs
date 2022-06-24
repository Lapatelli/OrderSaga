using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace OrderSaga.Host.Bootstrapping
{
    public static class ServiceBootstrapping
    {
        public static IServiceCollection BootstrapServices(this IServiceCollection services)
        {
            var config = BootstrapConfiguration();

            services
                .ConfigurateNHibernate(config)
                .ConfigureMassTransit(config);

            return services;
        }
 
        private static IConfigurationRoot BootstrapConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}
