using MemoryPack;

namespace GatiNetwork.Core.Packets
{
    [MemoryPackable]
    [PacketProtocol<PrivateC2SProtocolCodeGroup>(PrivateC2SProtocolCodeGroup.RequestConnect)]
    internal partial class RequestConnect : IPacket
    {
    }
}
