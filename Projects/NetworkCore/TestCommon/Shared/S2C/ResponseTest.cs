using MemoryPack;
using WebCore;
using WebCore.Shared;

namespace TestCommon.Shared.S2C;

[Packet(Opcode.RESPONSE_TEST)]
[MemoryPackable]
public partial class ResponseTest : IResPacket
{

}