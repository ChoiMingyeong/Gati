using System.Net.WebSockets;
using WebCore.Packet;

namespace WebCore.Socket.Client
{
    public class GatiClient<TRouter> where TRouter : IClientPacketRouter
    {
        private ClientWebSocket ClientWebSocket => (ClientWebSocket)_socket.Socket;
        private readonly GatiSocket _socket = new(new ClientWebSocket());
        private readonly TRouter _router;

        public GatiClient()
        {
            if (Activator.CreateInstance(typeof(TRouter), _socket) is TRouter router)
            {
                _router = router;
            }
            else
            {
                throw new InvalidOperationException($"Failed to create instance of {typeof(TRouter).Name}.");
            }
        }

        public async Task ConnectAsync(string uri)
        {
            await ClientWebSocket.ConnectAsync(new Uri(uri), CancellationToken.None);
            await _socket.Connected();
            _ = Task.Run(RouteLoopAsync);
        }

        public async Task SendAsync<TPacket>(TPacket packet) where TPacket : IPacket
        {
            await _socket.SendAsync(packet);
        }

        private async Task RouteLoopAsync()
        {
            while (_socket.IsConnected || false == _socket.ReceivedPackets.IsEmpty)
            {
                while (_socket.ReceivedPackets.TryDequeue(out var networkPacket))
                {
                    if (_router.ProtocolMethods.TryGetValue(networkPacket.Opcode, out var method))
                    {
                        await method(networkPacket.Payload);
                    }
                }

                await Task.Delay(10);
            }
        }
    }
}
