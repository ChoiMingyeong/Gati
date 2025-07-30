using MemoryPack;

namespace WebCore.Shared.C2S
{
    [Packet(Opcode.REQUEST_TEST)]
    [MemoryPackable]
    public partial class RequestTest : IReqPacket
    {
        public string Message { get; set; } = string.Empty;
    }
}