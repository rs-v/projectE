namespace Server.Players
{
    public class Player(string name, string id) : IPlayer
    {
        public string Name { get; set; } = name;
        public string Id { get; set; } = id;
        public int Score { get; set; } = 0;
        public bool IsReady { get; set; } = false;

        public int Position { get; set; } = 0;

        public int Money { get; set; } = 1000;

        public int Health { get; set; } = 100;
        public int Level { get; set; } = 1;
        public int Experience { get; set; } = 0;
        public int[] Inventory { get; set; } = new int[10];
        public int[] Quests { get; set; } = new int[5];
        public int[] Skills { get; set; } = new int[5];


    }
}
