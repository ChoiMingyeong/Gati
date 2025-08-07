using MemoryPack;
using System.Buffers;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Net.WebSockets;
using WebCore.Packet;

namespace WebCore.Socket
{
    public class GatiSocket
    {
        public event Func<Task>? OnConnected;
        public event Func<Task>? OnDisconnected;

        public bool IsConnected => Socket.State == WebSocketState.Open;

        public WebSocket Socket { get; init; }

        public ConcurrentQueue<NetworkPacket> ReceivedPackets { get; } = [];

        public GatiSocket(WebSocket socket)
        {
            Socket = socket;
        }

        private static NetworkPacket EncodePacket<TPacket>([NotNull] TPacket packet) where TPacket : IPacket
        {
            return new NetworkPacket
            {
                Opcode = packet.GetOpcode(),
                Payload = packet.Serialize(),
            };
        }

        public async Task SendAsync<TPacket>([NotNull] TPacket packet) where TPacket : IPacket
        {
            await Socket.SendAsync(
                MemoryPackSerializer.Serialize(EncodePacket(packet)),
                WebSocketMessageType.Binary,
                true,
                CancellationToken.None);
        }

        public async Task ReceiveAsync()
        {
            List<NetworkPacket> receivedPackets = [];
            byte[] buffer = ArrayPool<byte>.Shared.Rent(4096);

            try
            {
                while (Socket.State == WebSocketState.Open)
                {
                    var result = await Socket.ReceiveAsync(buffer, CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        break;
                    }

                    var rawData = new ReadOnlySpan<byte>(buffer, 0, result.Count);
                    if (MemoryPackSerializer.Deserialize<NetworkPacket>(rawData) is NetworkPacket networkPacket)
                    {
                        ReceivedPackets.Enqueue(networkPacket);
                    }
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        public async Task Connected()
        {
            if(null != OnConnected)
            {
                await OnConnected.Invoke();
            }

            _ = Task.Run(async () =>
            {
                while (Socket.State == WebSocketState.Open)
                {
                    await ReceiveAsync();
                }
            });
        }

        public async Task Disconnected()
        {
            if (null != OnDisconnected)
            {
                await OnDisconnected.Invoke();
            }

            await CloseAsync();
        }

        public async Task CloseAsync()
        {
            if (false == IsConnected)
            {
                return;
            }

            await Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
        }
    }

    public class ClientSocket : GatiSocket
    {
        private readonly ClientWebSocket _clientWebSocket = new();

        public ClientSocket(WebSocket socket) : base(socket)
        {
        }

        public async Task ConnectAsync(string uri)
        {
            await _clientWebSocket.ConnectAsync(new Uri(uri), CancellationToken.None);
            Console.WriteLine("[클라이언트] 연결됨");
            _ = Task.Run(async () => await ReceiveAsync());
        }
    }
}
