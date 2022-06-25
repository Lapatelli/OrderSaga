using Microsoft.Extensions.DependencyInjection;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using OrderSaga.DatabaseSchema.Migrations;
using System;

namespace OrderSaga.DatabaseSchema.Bootstrapping
{
    public static class MigrationBootstrapping
    {
        public static IServiceCollection BootstrapMigration(this IServiceCollection services)
        {
            var configuration = BootstrapConfiguration();

            services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSqlServer()
                    .WithGlobalConnectionString(configuration["ConnectionStrings:OrderSagaDB"])
                    .ScanIn(typeof(M202206250001_OrdersTableFormation).Assembly).For.Migrations()
                    .ScanIn(typeof(M202206250002_OrderItemsTableFormation).Assembly).For.Migrations())
                    .AddLogging(lb => lb.AddFluentMigratorConsole());

            services.AddScoped<IDbMigrationRunner, DbMigrationRunner>();

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
