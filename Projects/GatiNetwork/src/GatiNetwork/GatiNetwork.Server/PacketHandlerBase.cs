using GatiNetwork.Core;
using GatiNetwork.Core.PacketHandlers;
using GatiNetwork.Core.Sessions;

namespace GatiNetwork.Server
{
    public abstract class PacketHandlerBase<TPacket> : IPacketHandler<ClientSession, TPacket>
        where TPacket : IPacket
    {
        public abstract Task ExecuteAsync(ClientSession session, TPacket recPacket);
    }
}
