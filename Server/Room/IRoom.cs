using Server.Players;
namespace Server.Room
{
    public interface IRoom
    {
        int Id { get; }
        string Name { get; }
        int MaxPlayers { get; }
        int CurrentPlayers { get; }
        bool IsFull { get; }
        bool IsStarted { get; }
        void AddPlayer(IPlayer player);
        void RemovePlayer(IPlayer player);
        void StartGame();
        void EndGame();

    }
}
