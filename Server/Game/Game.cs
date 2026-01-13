using Server.Players;

namespace Server.Game
{
    public class Game : IGame
    {
        public int CurrentTurn { get; private set; }

        public int MaxTurns { get; private set; }

        public int CurrentPlayer { get; private set; }

        public List<Tile> Map { get; private set; }

        public List<IPlayer> Players { get; set; }

        public Game()
        {
            Map = [];
            CurrentPlayer = 0;
            MaxTurns = 10;
            CurrentTurn = 0;
            Players = [];
        }


        public void InitializeGame(List<IPlayer> players, int maxTurns)
        {
            Players = players;
            MaxTurns = maxTurns;
            CurrentTurn = 0;
            CurrentPlayer = 0;
            Map = [];
            for (var i = 0; i < 40; i++)
            {
                Map.Add(new Tile { Id = i, Type = TileType.Normal, Price = 100 });
            }
        }

        public void HandlePlayerAction(IPlayer player, string action)
        {
            if (action == "roll")
            {
                var steps = RollDice();
                MovePlayer(player, steps);
            }
            else if (action == "fight")
            {
                var tile = Map[player.Position];
                if (tile.Owner == null && player.Money >= tile.Price)
                {
                    tile.Owner = player;
                    player.Money -= tile.Price;
                }
            }
            else if (action == "skill")
            {
                var tile = Map[player.Position];
                if (tile.Owner == player)
                {
                    tile.Owner = null;
                    player.Money += tile.Price;
                }
            }

            NextTurn();
        }

        private void HandleTileEvent(IPlayer player)
        {
            var tile = Map[player.Position];
            if (tile.Type == TileType.Chance)
            {
                // Handle chance event
                player.Money += 50; // Example event
            }
            else if (tile.Type == TileType.Tax)
            {
                // Handle tax event
                player.Money -= 50; // Example event
            }
        }

        private void MovePlayer(IPlayer player, int steps)
        {
            player.Position = (player.Position + steps) % Map.Count;
            HandleTileEvent(player);
        }

        private void NextTurn()
        {
            CurrentTurn++;
            if (CurrentTurn >= MaxTurns)
            {
                // End game logic here
                return;
            }

            CurrentPlayer = (CurrentPlayer + 1) % Players.Count;
        }

        private static int RollDice()
        {
            var random = new Random();
            return random.Next(1, 7) + random.Next(1, 7);
        }


    }

    public class Tile
    {
        public int Id { get; set; }
        public TileType Type { get; set; }
        public IPlayer? Owner { get; set; }
        public int Price { get; set; }
    }

    public enum TileType
    {
        Normal,
        Start,
        Chance,
        Tax
    }
}
