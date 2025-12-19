using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRexamples.Data;

namespace SignalRexamples.Hubs
{
    public class BasicChatHub:Hub
    {
        private readonly ApplicationDbContext _context;

        public BasicChatHub(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task SendMessageToAll(string user,string message)
        {
            await Clients.All.SendAsync("MessageRecieved", user, message);
        }

        [Authorize]
        public async Task SendMessageToReceiver(string sender,string receiver, string message)
        {
            var userId = _context.Users.FirstOrDefault(u => u.Email.ToLower() == receiver.ToLower()).Id;

            if (!string.IsNullOrEmpty(userId))
            {
                await Clients.User(userId).SendAsync("MessageRecieved", sender, message); // asp as build libray for signalr so, message send automatically send the perticular userid.

            }
        }
    }
}
