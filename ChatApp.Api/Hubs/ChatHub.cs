using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace SignalRServer.Hubs
{
    
    public class ChatHub : Hub
    {

        [Authorize]
        public override Task OnConnectedAsync()
        {
            string name = Context.User.Identity.Name;
            Console.WriteLine("New Connection Started: " + Context.ConnectionId + " with user: " + name);
          //  Clients.All.SendAsync("NewConnection", "New Connection Successfull", Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            Console.WriteLine("Discconnected: " + Context.ConnectionId);
            Clients.All.SendAsync("DisconnectConnection", "Connection Destoryed", Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string message)
        {
            Console.WriteLine("Message received");
            await Clients.Client(Context.ConnectionId).SendAsync("SendMessage", message);
        }

        public async Task JoinRoom(string name)
        {
            Console.WriteLine("User Joined");
            await Clients.All.SendAsync("JoinRoom", name);
        }
    }
}