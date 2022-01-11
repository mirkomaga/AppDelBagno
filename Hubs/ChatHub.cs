

using Microsoft.AspNetCore.SignalR;

namespace AppDelBagno.Hubs
{
    public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public async Task Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            await Clients.All.SendAsync("broadcastMessage", name, message);
        }
    }

}
