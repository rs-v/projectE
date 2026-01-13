namespace Server.Commands
{
    public interface ICommand
    {
        string Name { get; }
        string Type { get; }
        string From { get; }

        string To { get; }



    }
}
