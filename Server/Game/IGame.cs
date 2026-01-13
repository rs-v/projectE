using Server.Players;
namespace Server.Game
{
    public interface IGame
    {
        int CurrentTurn { get; }
        int MaxTurns { get; }
        int CurrentPlayer { get; }

        // List<IPlayer> Players { get; }
        // void NextTurn();
        // int RollDice();
        // void MovePlayer(IPlayer player, int steps);

        // void HandlePlayerAction(IPlayer player, string action);

        // void HandleTileEvent(IPlayer player);


    }
}
