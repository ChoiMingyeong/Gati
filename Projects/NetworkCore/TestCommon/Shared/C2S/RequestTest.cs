using MemoryPack;
using WebCore;
using WebCore.Shared;

namespace TestCommon.Shared.C2S
{
    [Packet(Opcode.REQUEST_TEST)]
    [MemoryPackable]
    public partial class RequestTest : IReqPacket
    {
        public string Message { get; set; } = string.Empty;
    }
}