namespace Server.Players;

public interface IPlayer
{
    string Name { get; set; }
    string Id { get; set; }
    int Score { get; set; }
    bool IsReady { get; set; }

    int Position { get; set; }

    int Money { get; set; }

    int Health { get; set; }
    int Level { get; set; }
    int Experience { get; set; }
    int[] Inventory { get; set; }
    int[] Quests { get; set; }
    int[] Skills { get; set; }



}
