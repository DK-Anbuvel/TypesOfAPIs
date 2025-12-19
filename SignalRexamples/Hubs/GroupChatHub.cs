using Microsoft.AspNetCore.SignalR;
using SignalRexamples.Data;
using System.Security.Claims;

namespace SignalRexamples.Hubs
{
    public class GroupChatHub : Hub
    {
        private readonly ApplicationDbContext _db;

        public GroupChatHub(ApplicationDbContext db)
        {
            _db = db;
        }

        public override Task OnConnectedAsync()
        {
            var UserId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(UserId))
            {
                var user = _db.Users.FirstOrDefault(x => x.Id == UserId);
                var userName = user?.UserName;

                Clients.Users(HubConnections.OnlineUsers()).SendAsync("ReceivedUserConnected", UserId, userName);
                HubConnections.AddUserConnection(UserId, Context.ConnectionId);
            }

            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            //var UserId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            //if (HubConnections.HasUserConnection(UserId, Context.ConnectionId))
            //{
            //    var UserConnections = HubConnections.Users[UserId];
            //    UserConnections.Remove(Context.ConnectionId);
            //    if (UserConnections.Any())
            //     HubConnections.Users.Add(UserId, UserConnections);
            //}
            //if (!string.IsNullOrEmpty(UserId))
            //{
            //    var user = _db.Users.FirstOrDefault(x => x.Id == UserId);
            //    var userName = user?.UserName;

            //    Clients.Users(HubConnections.OnlineUsers()).SendAsync("ReceivedUserDisConnected", UserId, userName);
            //    HubConnections.AddUserConnection(UserId, Context.ConnectionId);
            //}

            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessageForChatRoom(int maxRoom,int roomId, string roomName,int type)
        {
            var UserId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _db.Users.FirstOrDefault(x => x.Id == UserId);
            var userName = user?.UserName;
            if(type ==1)
            await Clients.All.SendAsync("ReceiveAddRoomMsg",maxRoom,roomId,roomName,UserId,userName);
            else
                await Clients.All.SendAsync("ReceiveRemoveRoomMsg", maxRoom, roomId, roomName, UserId, userName);
        }
        public async Task SendRemoveMessageForChatRoom(int maxRoom, int roomId, string roomName, int type)
        {
            var UserId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _db.Users.FirstOrDefault(x => x.Id == UserId);
            var userName = user?.UserName;
            if (type == 1)
                await Clients.All.SendAsync("ReceiveAddRoomMsg", maxRoom, roomId, roomName, UserId, userName);
            else if (type ==2)
                await Clients.All.SendAsync("ReceiveRemoveRoomMsg", maxRoom, roomId, roomName, UserId, userName);
        }
        public async Task sendPublicMessage( int roomId, string meesage, string roomName)
        {
            var UserId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _db.Users.FirstOrDefault(x => x.Id == UserId);
            var userName = user?.UserName;
                await Clients.All.SendAsync("ReceivePublicMsg", roomId, UserId, userName,meesage, roomName);
        }
        public async Task sendPrivateMessage( string receiverId, string meesage, string receiverName)
        {
            var senderid = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _db.Users.FirstOrDefault(x => x.Id == senderid);
            var senderName = user?.UserName;
            var users = new string[] { senderid, receiverId };
                await Clients.Users(users).SendAsync("ReceivePrivateMsg", senderid, senderName, receiverId, meesage, Guid.NewGuid(),receiverName);
        }
    }

}
