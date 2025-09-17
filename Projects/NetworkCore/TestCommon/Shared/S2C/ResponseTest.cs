using GatiWeb.Core.Packet;
using MemoryPack;

namespace TestCommon.Shared.S2C;

[MemoryPackable]
[PacketProtocol(Opcode.RESPONSE_TEST)]
public partial class ResponseTest : IPacket
{
    public ResponseCode ResponseCode { get; set; }
}