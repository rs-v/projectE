using Server.Commands;
using Server.Players;
namespace Server.Room
{
    public interface IRoomManager
    {
        void CreateRoom(string name, int maxPlayers);
        void JoinRoom(int roomId, IPlayer player);
        void LeaveRoom(int roomId, IPlayer player);
        void StartGame(int roomId);

        ICommand HandleMessage(Dictionary<string, string> message);

    }

}
