using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderSaga.Host.Bootstrapping;
using System.Threading.Tasks;

namespace OrderSaga.Host
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .BootstrapServices()
                        .AddHostedService<MassTransitConsoleHostedService>();
                });
    }
}
