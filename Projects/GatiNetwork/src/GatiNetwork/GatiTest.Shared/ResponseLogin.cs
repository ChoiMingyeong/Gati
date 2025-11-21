using GatiNetwork.Core;
using GatiNetwork.Core.Packets;
using GatiTest.Shared.RecordStructs;
using MemoryPack;

namespace GatiTest.Shared
{
    [MemoryPackable]
    [PacketProtocol(S2CProtocolCode.ResponseLogin)]
    public partial class ResponseLogin : IPacket
    {
        public UserID UserID { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
