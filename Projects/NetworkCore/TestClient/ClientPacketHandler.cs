using TestCommon.Shared.S2C;
using WebCore.Packet;
using WebCore.Socket;
using WebCore.Socket.Client;

namespace TestClient
{
    public class TestClientRouter(GatiSocket socket) : IClientPacketRouter(socket)
    {
        [PacketHandler]
        private Task HandleResponseTest(ResponseTest packet)
        {
            Console.WriteLine(packet.ResponseCode);
            return Task.CompletedTask;
        }
    }
}
