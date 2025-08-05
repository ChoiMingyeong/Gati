using System.Net.WebSockets;
using TestCommon.Shared.C2S;
using WebCore.Packet;

namespace TestServer
{
    public class ServerPacketHandler : IPacketHandler
    {
        [PacketHandler]
        private Task OnRequestTest(WebSocket socket, RequestTest packet)
        {
            Console.WriteLine(packet.Message);
            return Task.CompletedTask;
        }
    }
}
