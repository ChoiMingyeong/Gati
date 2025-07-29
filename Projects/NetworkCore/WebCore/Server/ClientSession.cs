using System.Net.WebSockets;
using WebCore.Shared;

namespace WebCore.Server;

public class ClientSession
{
    public string Id { get; } = Guid.NewGuid().ToString();
    public WebSocket Socket { get; }
    public DateTime ConnectedAt { get; } = DateTime.UtcNow;

    public ClientSession(WebSocket socket)
    {
        Socket = socket;
    }

    public async Task SendAsync(Packet packet)
    {
        byte[] raw = MemoryPack.MemoryPackSerializer.Serialize(packet);
        await Socket.SendAsync(
            new ArraySegment<byte>(raw),
            WebSocketMessageType.Binary,
            true,
            CancellationToken.None);
    }
}
