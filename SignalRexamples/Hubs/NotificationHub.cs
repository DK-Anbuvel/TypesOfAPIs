using Microsoft.AspNetCore.SignalR;

namespace SignalRexamples.Hubs
{
    public class NotificationHub : Hub
    {
        private static List<string> NotificationMessage = new List<string>();

        public async Task<List<string>> SendNotification(string message)
        {
            if(!string.IsNullOrEmpty(message))
            NotificationMessage.Add(message);

            await Clients.All.SendAsync("NotificationStatus", NotificationMessage);

            return NotificationMessage;
        }
    }
}
