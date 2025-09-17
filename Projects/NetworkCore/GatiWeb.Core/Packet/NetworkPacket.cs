using MemoryPack;

namespace GatiWeb.Core.Packet
{
    [MemoryPackable]
    sealed public partial class NetworkPacket
    {
        public ushort Opcode { get; set; }

        public byte[] Payload { get; set; } = [];
    }
}
