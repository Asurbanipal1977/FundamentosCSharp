using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BlazorSignalR.Server
{
    public class TextHub : Hub
    {
        public async Task Send(string text)
        {
            await Clients.All.SendAsync("RecibirInformacion",text);
        }
    }
}
