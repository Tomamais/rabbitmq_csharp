using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace generichost
{
    public class MassTransitBusControlService : Microsoft.Extensions.Hosting.IHostedService
    {
        private readonly IBusControl busControl;

        public MassTransitBusControlService(IBusControl busControl)
        {
            this.busControl = busControl;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await busControl.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await busControl.StopAsync(cancellationToken);
        }
    }
}
