using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HostedServiceSignalR
{
    public class PopulationHostedService : IHostedService, IDisposable
    {
        private Timer _timer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(SendInfo, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        public void SendInfo(object State)
        {

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            //El timer se reinicia
            _timer.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
