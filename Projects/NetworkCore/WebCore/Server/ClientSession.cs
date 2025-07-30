using MemoryPack;
using System;
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

    public async Task Send<TPacket> (TPacket packet) where TPacket : IPacket
    {
        byte[] raw = MemoryPack.MemoryPackSerializer.Serialize(packet);
        var networkPacket = new NetworkPacket
        {
            Opcode = packet.GetOpcode(),
            Payload = raw
        };

        await Socket.SendAsync(
            MemoryPackSerializer.Serialize(networkPacket),
            WebSocketMessageType.Binary,
            true,
            CancellationToken.None);
    }
}
