using GatiNetwork.Core.Sessions;

namespace GatiNetwork.Core.PacketHandlers
{
    public interface IPacketHandler<TPacket> where TPacket : IPacket
    {
        Task Execute(IClientSession session, TPacket packet);
    }
}