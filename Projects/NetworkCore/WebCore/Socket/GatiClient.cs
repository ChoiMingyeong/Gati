using System.Net.WebSockets;

namespace WebCore.Socket
{
    public class GatiClient
    {
        private readonly GatiSocket _socket = new GatiSocket(new ClientWebSocket());
        private ClientWebSocket ClientWebSocket => (ClientWebSocket)_socket.Socket;

        public async Task ConnectAsync(string uri)
        {
            await ClientWebSocket.ConnectAsync(new Uri(uri), CancellationToken.None);
            _ = Task.Run(_socket.ReceiveAsync);
        }
    }

    //public abstract class GatiSocket<TPacketHandler> where TPacketHandler : IPacketHandler
    //{
    //    protected readonly TPacketHandler _packetHandler;
    //    protected readonly ConcurrentQueue<IPacket> _receivedPackets = [];

    //    public GatiSocket()
    //    {
    //        _packetHandler = Activator.CreateInstance<TPacketHandler>();
    //        _packetHandler.RegisterPacketHandlers();
    //    }

    //    private NetworkPacket EncodePacket<TPacket>([NotNull] TPacket packet) where TPacket : IPacket
    //    {
    //        return new NetworkPacket
    //        {
    //            Opcode = packet.GetOpcode(),
    //            Payload = packet.Serialize(),
    //        };
    //    }

    //    private TPacket? DecodePacket<TPacket>(in ReadOnlySpan<byte> buffer) where TPacket : IPacket
    //    {
    //        if (MemoryPackSerializer.Deserialize<NetworkPacket>(buffer) is not NetworkPacket networkPacket)
    //            return null;

    //        return MemoryPackSerializer.Deserialize<TPacket>(buffer);
    //    }

    //    public async Task SendAsync<TPacket>(WebSocket socket, [NotNull] TPacket packet) where TPacket : IPacket
    //    {
    //        await socket.SendAsync(
    //            MemoryPackSerializer.Serialize(EncodePacket(packet)),
    //            WebSocketMessageType.Binary,
    //            true,
    //            CancellationToken.None);
    //    }

    //    protected async Task ReceiveAsync(WebSocket socket)
    //    {
    //        byte[] buffer = ArrayPool<byte>.Shared.Rent(4096);
    //        try
    //        {
    //            while (socket.State == WebSocketState.Open)
    //            {
    //                var result = await socket.ReceiveAsync(buffer, CancellationToken.None);
    //                if (result.MessageType == WebSocketMessageType.Close)
    //                    break;

    //                var rawData = new ReadOnlySpan<byte>(buffer, 0, result.Count);
    //                if (MemoryPackSerializer.Deserialize<NetworkPacket>(rawData) is NetworkPacket networkPacket)
    //                {
    //                    await _packetHandler.RouteAsync(socket, networkPacket.Opcode, networkPacket.Payload);
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"[에러] {ex.Message}");
    //        }
    //        finally
    //        {
    //            ArrayPool<byte>.Shared.Return(buffer);
    //        }
    //    }
    //}
}
