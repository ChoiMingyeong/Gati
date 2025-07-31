using MemoryPack;
using System.Collections.Concurrent;
using System.Net;
using System.Net.WebSockets;
using TSID.Creator.NET;
using WebCore.Common;
using WebCore.Shared.C2S;
using WebCore.Shared.S2C;

namespace WebCore.Server;

public class ServerSocket : GatiWebSocket
{
    private readonly HttpListener _listener = new();
    private readonly ConcurrentDictionary<Tsid, ClientSession> _clients = new();

    public ServerSocket()
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
                _ = HandleWebSocketAsync(context);
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

        var buffer = new byte[2048];

        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                    break;

                var rawData = new ReadOnlySpan<byte>(buffer, 0, result.Count);
                if(MemoryPackSerializer.Deserialize<NetworkPacket>(rawData) is NetworkPacket networkPacket)
                {
                    if(networkPacket.Opcode == WebCore.Shared.C2S.Opcode.REQUEST_TEST)
                    {
                        if(MemoryPackSerializer.Deserialize<RequestTest>(networkPacket.Payload) is RequestTest request)
                        {
                            Console.WriteLine($"[요청] {session.Id} → {request.Message}");
                            // Echo back the request
                            await session.Send(new ResponseTest());
                        }
                    }
                }
            }
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
