using GatiNetwork.Core;
using GatiNetwork.Core.Packets;
using MemoryPack;

namespace GatiTest.Shared
{
    [MemoryPackable]
    [PacketProtocol<C2SProtocolCode>(C2SProtocolCode.RequestLogin)]
    public partial class RequestLogin : IPacket
    {
        public RequestLogin()
        {
        }
    }

}
