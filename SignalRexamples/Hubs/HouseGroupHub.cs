using Microsoft.AspNetCore.SignalR;

namespace SignalRexamples.Hubs
{
    public class HouseGroupHub :Hub
    {
        public static List<string> GroupsJoined { get; set; }= new List<string>(); // store the group in memory

        public async Task JoinHouse(string houseName)
        {
            if (!GroupsJoined.Contains(Context.ConnectionId + ":" + houseName))
            {
                GroupsJoined.Add(Context.ConnectionId + ":" + houseName);
                string houseList = "";
                foreach (var str in GroupsJoined) {
                    if (str.Contains(Context.ConnectionId)) {
                        houseList += str.Split(':')[1] + " ";
                    }
                }
                // send notification one who subscriber
                await Clients.Caller.SendAsync("subscriptionStatus", houseList, houseName.ToLower(), true);

                // send notification other then subscriber
                await Clients.Others.SendAsync("NotifyAddMember", houseName.ToLower());


                await Groups.AddToGroupAsync(Context.ConnectionId, houseName);

            }
        }
        public async Task LeaveHouse(string houseName)
        {
            if (GroupsJoined.Contains(Context.ConnectionId + ":" + houseName))
            {
                string houseList = "";
                foreach (var str in GroupsJoined)
                {
                    if (str.Contains(Context.ConnectionId))
                    {
                        houseList += str.Split(':')[1] + " ";
                    }
                }

                GroupsJoined.Remove(Context.ConnectionId + ":" + houseName);
                // send notification one who subscriber
                await Clients.Caller.SendAsync("subscriptionStatus", houseList, houseName.ToLower(), false);

                // send notification other then subscriber
                await Clients.Others.SendAsync("NotifyRemoveMember", houseName.ToLower());

                await Groups.RemoveFromGroupAsync(Context.ConnectionId, houseName);

            }
        }
        public async Task TrigerHouseNotify(string houseName)
        {
            await Clients.Group(houseName).SendAsync("tiggerNotify", houseName);
        }
    }
}
