using GatiNetwork.Core.Sessions;

namespace GatiNetwork.Core.PacketHandlers
{
    public interface IPacketHandler<TPacket>
        where TPacket : IPacket
    {
        Task ExecuteAsync(ClientSession session, TPacket recPacket);
    }
}
