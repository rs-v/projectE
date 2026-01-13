using Serilog;
namespace Server;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Async(a => a.Console())
            // .WriteTo.File("logs/game_server.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();
        Log.Information("Starting the game server...");
        GameMain.Start();
    }
}
