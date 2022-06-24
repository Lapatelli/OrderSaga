using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using OrderSaga.Contracts;
using System;

namespace OrderSaga.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);

            services.AddMassTransit(busConfig =>
            {
                busConfig.AddRequestClient<CreateOrder>(new Uri(Configuration["MassTransit:Exchanges:CreateOrderExchange"]));
                busConfig.AddRequestClient<ChangeOrderStatus>(new Uri(Configuration["MassTransit:Exchanges:ChangeOrderStatusExchange"]));

                busConfig.UsingRabbitMq((context, busFactoryConfig) =>
                {
                    busFactoryConfig.Host(Configuration["RabbitMq:Host"], "/", hostConfig =>
                    {
                        hostConfig.Username(Configuration["RabbitMq:Credentials:Username"]);
                        hostConfig.Password(Configuration["RabbitMq:Credentials:Password"]);
                    });

                    busFactoryConfig.ConfigureEndpoints(context);
                });
            });

            services.AddOpenApiDocument(cfg => cfg.PostProcess = d => d.Info.Title = "OrderSaga API");

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
