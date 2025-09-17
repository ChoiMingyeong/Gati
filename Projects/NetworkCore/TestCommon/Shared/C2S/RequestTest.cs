using GatiWeb.Core.Packet;
using MemoryPack;

namespace TestCommon.Shared.C2S
{
    [MemoryPackable]
    [PacketProtocol(Opcode.REQUEST_TEST)]
    public partial class RequestTest : IPacket
    {
        public string Message { get; set; } = string.Empty;
    }
}