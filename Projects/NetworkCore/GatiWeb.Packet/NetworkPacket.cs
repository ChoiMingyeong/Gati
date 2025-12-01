using MemoryPack;

namespace GatiWeb.Packet
{
    [MemoryPackable]
    sealed public partial class NetworkPacket
    {
        public ushort Opcode { get; set; }

        public byte[] Payload { get; set; } = [];
    }
}
