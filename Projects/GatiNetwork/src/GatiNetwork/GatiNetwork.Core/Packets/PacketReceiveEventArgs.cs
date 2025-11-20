using GatiNetwork.Core.RecordStructs;

namespace GatiNetwork.Core.Packets
{
    public class PacketReceiveEventArgs(SessionID sessionID, IPacket packet) : EventArgs
    {
        public SessionID SessionID { get; } = sessionID;

        public IPacket Packet { get; } = packet;
    }
}
