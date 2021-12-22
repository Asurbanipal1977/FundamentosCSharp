using HostedServiceSignalR.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HostedServiceSignalR
{
    public class PopulationHostedService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IHubContext<PopulationHub> _hubContext;

        public PopulationHostedService (IHubContext<PopulationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(SendInfo, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        public void SendInfo(object State)
        {
            IEnumerable<Alumno> alumnos;

            using (var context = new EFContext())
            {
                alumnos = context.Alumnos.ToList();
            }

            _hubContext.Clients.All.SendAsync("listadoAlumnos", alumnos);
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
