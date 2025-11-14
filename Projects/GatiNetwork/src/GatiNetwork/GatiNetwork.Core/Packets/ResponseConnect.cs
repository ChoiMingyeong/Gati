using GatiNetwork.Core.RecordStructs;
using MemoryPack;

namespace GatiNetwork.Core.Packets
{
    [MemoryPackable]
    [PacketProtocol<PrivateS2CProtocolCodeGroup>(PrivateS2CProtocolCodeGroup.ResponseConnect)]
    internal partial class ResponseConnect : IPacket
    {
        internal SessionID SessionID { get; set; }
    }
}
