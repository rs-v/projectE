namespace Server.Commands
{
    public class CreateRoomCommand(string roomName, int maxPlayers) : ICommand
    {
        public string Name { get; set; } = "create_room";
        public string From { get; set; } = "Client";
        public string To { get; set; } = "Server";
        public string Type { get; set; } = "room";

        public string RoomName { get; set; } = roomName;
        public int MaxPlayers { get; set; } = maxPlayers;
    }
}
