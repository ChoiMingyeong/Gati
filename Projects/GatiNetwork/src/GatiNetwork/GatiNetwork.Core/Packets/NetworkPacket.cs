using GatiNetwork.Core.RecordStructs;
using MemoryPack;

namespace GatiNetwork.Core
{
    [MemoryPackable]
    public sealed partial class NetworkPacket
    {
        public ProtocolCode ProtocolCode { get; init; }

        public byte[] Payload { get; init; } = [];
    }
}
