using System.Collections.Concurrent;
using System.Net;
using System.Net.WebSockets;
using WebCore.Shared;

namespace WebCore.Server;

public class WebSocketServer
{
    private readonly HttpListener _listener = new();
    private readonly ConcurrentDictionary<string, ClientSession> _clients = new();

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
                var packet = MemoryPack.MemoryPackSerializer.Deserialize<Packet>(rawData);
                if (packet != null)
                {
                    Console.WriteLine($"[패킷 수신] {packet.Opcode} ({packet.Payload.Length} bytes)");

                    // TODO: PacketRouter.Handle(session, packet)
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
