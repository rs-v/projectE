using System.Net;
using System.Net.WebSockets;
using Server.Room;
using Server.Commands;
using Newtonsoft.Json;
using Serilog;
using System.Collections.Concurrent;

namespace Server.NetWork.WebSocket
{
    public class WebSocketServer : IWebSocketServer
    {
        private readonly HttpListener _httpListener;
        private readonly byte[] _pongResponse;
        private readonly ConcurrentDictionary<Guid, System.Net.WebSockets.WebSocket> _activeSockets = new();

        public WebSocketServer(string url)
        {
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add(url);
            _pongResponse = System.Text.Encoding.UTF8.GetBytes("PONG");
            // ThreadPool.SetMaxThreads(10000, 10000);
        }
        public async Task StartAsync()
        {
            _httpListener.Start();
            while (true)
            {
                try
                {
                    var context = await _httpListener.GetContextAsync();
                    if (context.Request.IsWebSocketRequest)
                    {
                        var webSocketContext = await context.AcceptWebSocketAsync(null);
                        var socketId = Guid.NewGuid();
                        _activeSockets.TryAdd(socketId, webSocketContext.WebSocket);
                        _ = HandleWebSocketAsync(webSocketContext.WebSocket, socketId);
                    }
                    else
                    {
                        context.Response.StatusCode = 404;
                        context.Response.Close();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Error accepting WebSocket connection: {ex.Message}");

                }

            }
        }

        public void Stop()
        {
            _httpListener.Stop();
            foreach (var socket in _activeSockets.Values)
            {
                if (socket.State == WebSocketState.Open || socket.State == WebSocketState.CloseReceived)
                {
                    socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Server stopping", CancellationToken.None).Wait();
                }
                socket.Dispose();
            }
            _activeSockets.Clear();
            Log.Information("WebSocket server stopped.");
        }

        public void SendMessage(string message)
        {
            var messageBytes = System.Text.Encoding.UTF8.GetBytes(message);
            foreach (var socket in _activeSockets.Values)
            {
                if (socket.State == WebSocketState.Open)
                {
                    socket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None).Wait();
                }
            }
        }

        public async Task<ICommand> HandleMessage(string message)
        {
            try
            {
                Log.Information($"Received message: {message}");
                var jMessage = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);
                if (jMessage != null && jMessage.TryGetValue("type", out var value) && value == "room")
                {
                    return await Task.FromResult(RoomManager.Instance.HandleMessage(jMessage));
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to handle message: {message}. Error: {ex.Message}");
            }

            return await Task.FromResult(new EchoCommand("Invalid message format"));
        }

        public async Task HandleWebSocketAsync(System.Net.WebSockets.WebSocket webSocket, Guid socketId)
        {
            var buffer = new byte[1024];
            var pingTimeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(30)); // 30秒超时
            try
            {
                while (webSocket.State == WebSocketState.Open)
                {
                    using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(pingTimeoutCts.Token);
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), linkedCts.Token);

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var message = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
                        // Log.Information(message);
                        if (message == "PING")
                        {
                            await webSocket.SendAsync(new ArraySegment<byte>(_pongResponse), WebSocketMessageType.Text, true, CancellationToken.None);
                            pingTimeoutCts.Cancel(); // 重置超时计时器
                            pingTimeoutCts.Dispose(); // 释放旧的 CancellationTokenSource
                            pingTimeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(30)); // 重新设置30秒超时
                            continue;
                        }

                        var cmd = await Task.Run(() => HandleMessage(message));
                        var response = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(cmd));
                        await webSocket.SendAsync(new ArraySegment<byte>(response), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                    }
                }

                if (webSocket.State == WebSocketState.Aborted || webSocket.State == WebSocketState.Closed)
                {
                    Log.Warning($"WebSocket connection {socketId} closed unexpectedly.");
                }
            }
            catch (OperationCanceledException) when (pingTimeoutCts.Token.IsCancellationRequested)
            {
                Log.Warning($"WebSocket connection {socketId} closed due to ping timeout.");
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Ping timeout", CancellationToken.None);
            }
            catch (OperationCanceledException ex)
            {
                Log.Warning($"WebSocket connection {socketId} canceled: {ex.Message}");
            }
            catch (Exception ex)
            {
                Log.Error($"Error handling WebSocket: {ex.Message}");
            }
            finally
            {
                pingTimeoutCts.Dispose();
                if (_activeSockets.TryRemove(socketId, out var socket))
                {
                    if (socket.State != WebSocketState.Closed && socket.State != WebSocketState.Aborted)
                    {
                        await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                    }
                    await socket.CloseAsync(WebSocketCloseStatus.InternalServerError, "Closing", CancellationToken.None);
                    socket.Dispose();
                }

                Log.Information($"WebSocket connection {socketId} closed.");
                var wsNumber = GetActiveSocketCount();
                Log.Information($"WebSocket active connections: {wsNumber}");
            }
        }


        private int GetActiveSocketCount()
        {
            return _activeSockets.Count;
        }
    }
}
