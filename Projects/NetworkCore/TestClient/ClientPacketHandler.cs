using GatiWeb.Core.Packet;
using GatiWeb.Core.Socket;
using GatiWeb.Core.Socket.Client;
using TestCommon.Shared.S2C;

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
