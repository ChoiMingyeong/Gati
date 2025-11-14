using GatiNetwork.Core;
using MemoryPack;

namespace GatiTest.Shared
{
    [MemoryPackable]
    [PacketProtocol(C2SProtocolCode.RequestLogin)]
    public partial class RequestLogin : IPacket
    {
        public RequestLogin()
        {
        }
    }

}
