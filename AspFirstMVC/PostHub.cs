using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace AspFirstMVC
{
    public class PostHub : Hub
    {
        public async Task Send(int Id, string Title)
        {
            await Clients.All.SendAsync("Receive", Id, Title);
        }
    }
}
