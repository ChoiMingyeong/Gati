using MemoryPack;

namespace GatiNetwork.Core
{
    [MemoryPackable]
    internal sealed partial class NetworkPacket
    {
        public ushort ProtocolCode { get; }

        public byte[] Payload { get; } = [];
    }
}
