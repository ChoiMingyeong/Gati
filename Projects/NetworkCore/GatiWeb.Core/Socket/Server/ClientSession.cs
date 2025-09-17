using GatiWeb.Core.Packet;
using System.Net.WebSockets;
using TSID.Creator.NET;

namespace GatiWeb.Core.Socket.Server;

public class ClientSession
{
    public Tsid ID { get; } = TsidCreator.GetTsid();

    private readonly GatiSocket _socket;

    public ClientSession(WebSocket socket) => _socket = new GatiSocket(socket);

    public async Task Send<TPacket>(TPacket packet) where TPacket : IPacket => await _socket.SendAsync(packet);

    public async Task Connected() => await _socket.Connected();

    public async Task Disconnected()
    {
        await _socket.Disconnected();
        Console.WriteLine($"[연결 종료] {ID}");
    }

    public bool Routable => _socket.IsConnected || false == _socket.ReceivedPackets.IsEmpty;

    public IEnumerable<NetworkPacket> GetReceivedPackets()
    {
        while (_socket.ReceivedPackets.TryDequeue(out var packet))
        {
            yield return packet;
        }
    }
}
