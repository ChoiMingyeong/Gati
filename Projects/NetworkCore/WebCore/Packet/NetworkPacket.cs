using MemoryPack;

namespace WebCore.Packet
{
    [MemoryPackable]
    sealed internal partial class NetworkPacket
    {
        public ushort Opcode { get; set; }

        public byte[] Payload { get; set; } = [];
    }
}
