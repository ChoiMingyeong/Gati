using MemoryPack;

namespace WebCore.Shared.S2C
{
    [Packet(Opcode.RESPONSE_TEST)]
    [MemoryPackable]
    public partial class ResponseTest : IResPacket
    {

    }
}