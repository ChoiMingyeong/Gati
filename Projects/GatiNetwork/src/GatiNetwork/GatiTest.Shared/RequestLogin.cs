using GatiNetwork.Core;
using GatiNetwork.Core.Packets;
using MemoryPack;

namespace GatiTest.Shared
{
    [MemoryPackable]
    [PacketProtocol(C2SProtocolCode.RequestLogin)]
    public partial class RequestLogin : IPacket
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
