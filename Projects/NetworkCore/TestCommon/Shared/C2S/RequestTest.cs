using MemoryPack;
using WebCore.Packet;

namespace TestCommon.Shared.C2S
{
    [Packet(Opcode.REQUEST_TEST)]
    [MemoryPackable]
    public partial class RequestTest : IPacket
    {
        public string Message { get; set; } = string.Empty;
    }
}