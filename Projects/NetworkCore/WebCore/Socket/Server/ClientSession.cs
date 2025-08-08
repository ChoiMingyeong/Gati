using MemoryPack;
using System.Net.WebSockets;
using WebCore.Packet;

namespace WebCore.Socket.Server;

public class ClientSession
{
    public GatiSocket Socket { get; }

    public ClientSession(WebSocket socket)
    {
        Socket = new GatiSocket(socket);
    }

    public async Task Send<TPacket> (TPacket packet) where TPacket : IPacket
    {
        await Socket.SendAsync(packet);
    }
}
