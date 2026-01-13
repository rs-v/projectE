using Serilog;

namespace Server.Loops
{
    public class PlayerLoop : ILoop
    {
        public bool IsRunning { get; private set; } = true;

        public void Tick()
        {
            // Log.Information("Player loop is running... wait for 1 second.");
            Thread.Sleep(1000);
        }
    }
}
