using GatiNetwork.Core.RecordStructs;

namespace GatiNetwork.Core.PacketHandlers
{
    public interface IPacketHandlerRegistry<TPacket> where TPacket : IPacket
    {
        Dictionary<ProtocolCode, IPacketHandler<TPacket>> PacketHandlers { get; }
    }
}
