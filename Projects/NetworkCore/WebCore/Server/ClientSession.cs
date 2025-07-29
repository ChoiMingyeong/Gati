using System.Net.WebSockets;
using TSID.Creator.NET;
using WebCore.Shared;

namespace WebCore.Server;

public class ClientSession
{
    public Tsid Id { get; } = TsidCreator.GetTsid();
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
