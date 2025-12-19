namespace SignalRexamples.Models
{
    public static class Voting
    {
        static Voting()
        {
            DealthyHallowRace = new Dictionary<string, int>();
            DealthyHallowRace.Add(Stone, 0);
            DealthyHallowRace.Add(Wand, 0);
            DealthyHallowRace.Add(Cloak, 0);
        }
        public const string Wand = "wand";
        public const string Cloak= "cloak";
        public const string Stone = "stone";
        public static Dictionary<string,int> DealthyHallowRace { get; }
    }
}
