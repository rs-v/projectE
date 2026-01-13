using Server.Players;

namespace Server.Room
{
    public class Room(string name, int maxPlayers) : IRoom
    {
        public string Name { get; private set; } = name;
        public int Id { get; private set; } = new Random().Next(1, 10000); // Unique ID for the room
        public int MaxPlayers { get; private set; } = maxPlayers;
        public int CurrentPlayers { get; private set; } = 0;
        public bool IsFull => CurrentPlayers >= MaxPlayers;
        public bool IsStarted { get; private set; } = false;
        private List<IPlayer> _players = [];
        public void StartGame()
        {
            if (!IsStarted && CurrentPlayers > 1)
            {
                IsStarted = true;
                // Logic to start the game
            }
        }
        public void AddPlayer(IPlayer player)
        {
            if (!IsFull && !IsStarted)
            {
                CurrentPlayers++;
                _players.Add(player);

            }
        }

        public void RemovePlayer(IPlayer player)
        {
            if (_players.Contains(player))
            {
                CurrentPlayers--;
                _players.Remove(player);
            }
        }

        public void EndGame()
        {
            if (IsStarted)
            {
                IsStarted = false;
            }
        }

    }
}
