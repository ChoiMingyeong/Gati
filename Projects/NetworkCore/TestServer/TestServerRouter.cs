using TestCommon.Shared.C2S;
using TestCommon.Shared.S2C;
using WebCore.Packet;
using WebCore.Socket.Server;

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
