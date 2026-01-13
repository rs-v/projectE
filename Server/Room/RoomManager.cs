using Server.Commands;
using Server.Players;
namespace Server.Room
{


    public class RoomManager : IRoomManager
    {
        private readonly List<IRoom> _rooms = [];
        public static readonly Lazy<RoomManager> _ = new(() => new RoomManager());
        public static RoomManager Instance => _.Value;

        public void CreateRoom(string name, int maxPlayers)
        {
            var room = new Room(name, maxPlayers);
            _rooms.Add(room);
        }
        public void JoinRoom(int roomId, IPlayer player)
        {
            var room = _rooms.FirstOrDefault(r => r.Id == roomId);
            if (room != null && !room.IsFull)
            {
                room.AddPlayer(player);
            }
        }
        public void LeaveRoom(int roomId, IPlayer player)
        {
            var room = _rooms.FirstOrDefault(r => r.Id == roomId);
            room?.RemovePlayer(player);
        }
        public void StartGame(int roomId)
        {
            var room = _rooms.FirstOrDefault(r => r.Id == roomId);
            if (room != null && !room.IsStarted && room.CurrentPlayers > 1)
            {
                room.StartGame();
            }
        }

        public ICommand HandleMessage(Dictionary<string, string> message)
        {
            Console.WriteLine($"Received message: {message}");
            if (message["name"] == "create_room")
            {
                var roomName = message["room_name"];
                var maxPlayers = int.Parse(message["max_players"]);
                CreateRoom(roomName, maxPlayers);
                return new CreateRoomCommand(roomName, maxPlayers);
            }
            else if (message["name"] == "fetch_rooms")
            {
                var rooms = _rooms.ToList();
                return new FetchRoomsCommand(rooms);
            }
            return new EchoCommand("hi");

        }


    }
}
