using MemoryPack;
using WebCore.Packet;

namespace TestCommon.Shared.S2C;

[Packet(Opcode.RESPONSE_TEST)]
[MemoryPackable]
public partial class ResponseTest : IPacket
{
    public ResponseCode ResponseCode { get; set; }
}