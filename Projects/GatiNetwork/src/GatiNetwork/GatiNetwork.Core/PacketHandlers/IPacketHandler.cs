using GatiNetwork.Core.Sessions;

namespace GatiNetwork.Core.PacketHandlers
{
    public interface IPacketHandler<TSession, TPacket>
        where TSession : ISession
        where TPacket : IPacket
    {
        Task ExecuteAsync(TSession session, TPacket recPacket);
    }
}
