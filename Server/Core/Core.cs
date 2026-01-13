using System.Net.WebSockets;
using Server.Loops;
using Server.NetWork.WebSocket;
using Server.Room;

namespace Server.Core;

public class Core
{
    private readonly List<ILoop> _loops = new List<ILoop>();
    private List<IRoomManager> _rooms = new List<IRoomManager>();

    private WebSocketServer? _webSocketServer;

    public Core()
    {
        AddLoop(new PlayerLoop());


    }
    public void AddLoop(ILoop loop)
    {
        _loops.Add(loop);
    }
    public void Start()
    {
        _webSocketServer = new WebSocketServer("http://127.0.0.1:5000/");
        _ = _webSocketServer.StartAsync();

        while (true)
        {
            foreach (var loop in _loops)
            {
                loop.Tick();
            }
        }
    }




}
