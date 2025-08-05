using System.Collections.Concurrent;
using System.Net;
using System.Net.WebSockets;
using TSID.Creator.NET;
using WebCore.Network;
using WebCore.Shared.C2S;

namespace WebCore.Socket.Server;

public class ServerSocket : GatiSocket
{
    private readonly HttpListener _listener = new();
    private readonly ConcurrentDictionary<Tsid, ClientSession> _clients = new();

    public ServerSocket(PacketRouter packetRouter) : base(packetRouter)
    {
    }

    public async Task StartAsync(string prefix = "http://localhost:8080/ws/")
    {
        _listener.Prefixes.Add(prefix);
        _listener.Start();
        Console.WriteLine($"HttpListener Start: {prefix}");

        while (true)
        {
            var context = await _listener.GetContextAsync();

            if (context.Request.IsWebSocketRequest)
            {
                _ = HandleWebSocketAsync(context);
            }
            else
            {
                context.Response.StatusCode = 400;
                context.Response.Close();
            }
        }
    }

    private async Task HandleWebSocketAsync(HttpListenerContext context)
    {
        var wsContext = await context.AcceptWebSocketAsync(null);
        var socket = wsContext.WebSocket;
        var session = new ClientSession(socket);
        _clients[session.Id] = session;

        Console.WriteLine($"[클라이언트 접속] {session.Id}");

        try
        {
            await ReceiveAsync(socket);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[에러] {session.Id} → {ex.Message}");
        }
        finally
        {
            _clients.TryRemove(session.Id, out _);
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "서버 종료", CancellationToken.None);
            Console.WriteLine($"[연결 종료] {session.Id}");
        }
    }

}
