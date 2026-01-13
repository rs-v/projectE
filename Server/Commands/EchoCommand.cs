namespace Server.Commands
{
    public class EchoCommand(string message) : ICommand
    {
        public string Name { get; set; } = "echo";
        public string From { get; set; } = "Client";
        public string To { get; set; } = "Server";
        public string Type { get; set; } = "echo";
        public string Message { get; set; } = message;
    }
}
