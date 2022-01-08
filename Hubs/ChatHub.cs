using Microsoft.AspNetCore.SignalR;

namespace AppDelBagno.Hubs
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.SendAsync("broadcastMessage", name, message);
        }
    }
}
