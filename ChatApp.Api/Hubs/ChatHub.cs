using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using ChatApp.Infrastructure.Services;
using ChatApp.Core.Domain;
using ChatApp.Infrastructure.DTO;

namespace SignalRServer.Hubs
{
    
    public class ChatHub : Hub
    {
        private readonly IUserService _userService;
        public ChatHub(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        public override Task OnConnectedAsync()
        {
            string name = Context.User.Identity.Name;
            _userService.AddConnection(name, Context.ConnectionId );
            Console.WriteLine("New Connection Started: " + Context.ConnectionId + " with user: " + name);
          //  Clients.All.SendAsync("NewConnection", "New Connection Successfull", Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            Console.WriteLine("Discconnected: " + Context.ConnectionId);
           // Clients.All.SendAsync("DisconnectConnection", "Connection Destoryed", Context.ConnectionId);
            _userService.RemoveConnection( Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
        [Authorize]
        public async Task SendMessage(string message, Guid receiverId)
        {
            Console.WriteLine("Message received");
            var Connections = await _userService.GetUserConnections(receiverId);
            foreach (ConnectionDto connection  in Connections)
            {
                await Clients.Client(connection.ConnectionId).SendAsync("ReciveMessage", message,Context.User.Identity.Name);
            }
            //await Clients.Client(Context.ConnectionId).SendAsync("ReciveMessage", message);
        }

        public async Task JoinRoom(string name)
        {
            Console.WriteLine("User Joined");
            await Clients.All.SendAsync("JoinRoom", name);
        }
    }
}