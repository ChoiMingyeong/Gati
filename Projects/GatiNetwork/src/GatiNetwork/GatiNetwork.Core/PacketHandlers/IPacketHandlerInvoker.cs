using GatiNetwork.Core.Sessions;

namespace GatiNetwork.Core.PacketHandlers
{
    public interface IPacketHandlerInvoker
    {
        Task ExecuteAsync(ClientSession session, IPacket packet);
    }
}
