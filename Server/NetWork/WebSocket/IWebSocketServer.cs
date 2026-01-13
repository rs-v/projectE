using Server.Commands;
namespace Server.NetWork.WebSocket
{
    public interface IWebSocketServer
    {

        Task StartAsync();
        void Stop();
        void SendMessage(string message);
        Task<ICommand> HandleMessage(string message);

        Task HandleWebSocketAsync(System.Net.WebSockets.WebSocket webSocket, Guid socketId);
    }
}
