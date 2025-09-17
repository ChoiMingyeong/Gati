using GatiWeb.Core.Packet;
using GatiWeb.Core.Socket.Server;
using TestCommon.Shared.C2S;
using TestCommon.Shared.S2C;

namespace TestServer
{
    public class TestServerRouter : IServerPacketRouter
    {
        [PacketHandler]
        private async Task OnRequestTest(ClientSession session, RequestTest packet)
        {
            Console.WriteLine($"{session.ID}:{packet.Message}");
            await session.Send(new ResponseTest());
        }
    }
}
