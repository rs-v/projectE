namespace Server.Loops
{
    public interface ILoop
    {
        void Tick();
        bool IsRunning { get; }
    }
}
