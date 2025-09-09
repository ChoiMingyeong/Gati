using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TestCommon.Shared.S2C;
using WebCore.Packet;
using WebCore.Socket;
using WebCore.Socket.Client;

namespace Assets.Scripts.Network
{
    public class DagaClientRouter : IClientPacketRouter
    {
        public DagaClientRouter(GatiSocket socket) : base(socket)
        {
        }

        [PacketHandler]
        private Task HandleResponseTest(ResponseTest packet)
        {
            Console.WriteLine(packet.ResponseCode);
            Debug.Write(packet.ResponseCode);
            return Task.CompletedTask;
        }
    }
}
