using System.Net.WebSockets;
using WebCore.Network;
using WebCore.Shared.C2S;

namespace WebCore.Socket.Client;
public class ClientSocket : GatiSocket
{
    private readonly ClientWebSocket _socket = new();
    private Uri _uri;

    public ClientSocket(PacketRouter packetRouter) : base(packetRouter)
    {
        //_socket.Options.KeepAliveInterval = TimeSpan.FromSeconds(30);
        //_socket.Options.SetBuffer(4096, 4096);
    }

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