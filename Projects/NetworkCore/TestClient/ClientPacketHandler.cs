using System.Net.WebSockets;
using TestCommon.Shared.S2C;
using WebCore.Packet;

namespace TestClient
{
    public class ClientPacketHandler : IPacketHandler
    {
        [PacketHandler]
        private Task OnResponseTest(WebSocket socket, ResponseTest packet)
        {
            Console.WriteLine(packet.ResponseCode);
            return Task.CompletedTask;
        }
    }
}
