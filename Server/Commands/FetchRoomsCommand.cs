using Server.Room;

namespace Server.Commands
{
    public class FetchRoomsCommand(List<IRoom> rooms) : ICommand
    {
        public string Name { get; set; } = "fetch_rooms";
        public string From { get; set; } = "Client";
        public string To { get; set; } = "Server";

        public string RoomName { get; set; } = string.Empty;
        public int MaxPlayers { get; set; }

        public string Type { get; set; } = "room";

        public List<IRoom> Rooms { get; set; } = rooms;

    }
}
