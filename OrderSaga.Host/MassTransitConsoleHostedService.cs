using MassTransit;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace OrderSaga.Host
{
    public class MassTransitConsoleHostedService : IHostedService
    {
        private readonly IBusControl _busControl;

        public MassTransitConsoleHostedService(IBusControl busControl)
        {
            _busControl = busControl;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _busControl.StartAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _busControl.StopAsync(cancellationToken);
        }
    }
}
