using System.Net.WebSockets;
using WebCore.Packet;

namespace WebCore.Socket.Client;

public class ClientSocket<TPacketHandler> : GatiSocket<TPacketHandler>
    where TPacketHandler : IPacketHandler
{
    private readonly ClientWebSocket _socket = new();
    private Uri _uri;

    public async Task ConnectAsync(string uri)
    {
        _uri = new Uri(uri);
        await _socket.ConnectAsync(_uri, CancellationToken.None);
        Console.WriteLine("[클라이언트] 연결됨");

        _ = Task.Run(async () => await ReceiveAsync(_socket));
    }

    public async Task Send<TPacket>(TPacket packet) where TPacket : IPacket
    {
        await SendAsync(_socket, packet);
    }
}