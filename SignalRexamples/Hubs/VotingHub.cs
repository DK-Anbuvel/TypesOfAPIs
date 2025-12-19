using Microsoft.AspNetCore.SignalR;
using SignalRexamples.Models;

namespace SignalRexamples.Hubs
{
    public class VotingHub : Hub
    {
        public Dictionary<string,int> GetRaceStatus()
        {
            return Voting.DealthyHallowRace;
        }
    }
}
