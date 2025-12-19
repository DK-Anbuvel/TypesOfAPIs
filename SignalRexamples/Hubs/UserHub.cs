using Microsoft.AspNetCore.SignalR;

namespace SignalRexamples.Hubs
{
    public class UserHub :Hub  
    {
        public static int TotalViews {  get; set; } = 0;

        public static int CurrentTotalViewers { get; set; } = 0;
        public async Task NewWindowLoaded()  // return type void
        {
            TotalViews++;
            // send update to all clients to the total views have been updated.

            await Clients.All.SendAsync("updateTotalViews", TotalViews);
        } 
        public async Task<string> NewWindowLoaded1(string name) // return string
        {
          //  TotalViews++;
            // send update to all clients to the total views have been updated.

            await Clients.All.SendAsync("updateTotalViews", TotalViews);
            return $"{name} : {TotalViews}";
        }
        public override Task OnConnectedAsync()
        {
            CurrentTotalViewers++;
            Clients.All.SendAsync("updateToUsers", CurrentTotalViewers).GetAwaiter().GetResult();// it will await for result

            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            CurrentTotalViewers--;
            Clients.All.SendAsync("updateToUsers", CurrentTotalViewers).GetAwaiter().GetResult();// it will await for result

            return base.OnDisconnectedAsync(exception);
        }

    }
}
