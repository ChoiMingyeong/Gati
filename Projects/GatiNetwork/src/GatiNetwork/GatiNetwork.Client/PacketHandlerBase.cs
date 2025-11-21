using GatiNetwork.Core;
using GatiNetwork.Core.PacketHandlers;
using GatiNetwork.Core.Sessions;

namespace GatiNetwork.Client
{
    public abstract class PacketHandlerBase<TPacket> : IPacketHandler<ServerSession, TPacket>
        where TPacket : IPacket
    {
        public abstract Task ExecuteAsync(ServerSession session, TPacket recPacket);
    }
}
