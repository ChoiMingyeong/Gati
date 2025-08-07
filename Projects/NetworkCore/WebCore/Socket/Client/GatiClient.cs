using System.Net.WebSockets;
using WebCore.Packet;

namespace WebCore.Socket.Client
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

        public async Task SendAsync<TPacket>(TPacket packet) where TPacket : IPacket
        {
            await _socket.SendAsync(packet);
        }
    }
}
