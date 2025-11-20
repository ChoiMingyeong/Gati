using GatiNetwork.Core.RecordStructs;
using GatiNetwork.Core.Transport;

namespace GatiNetwork.Core.PacketHandlers
{
    public static class PacketChannelFactory
    {
        public static readonly Dictionary<ProtocolCode, Type> PacketTypes = [];

        public static void Initialize(IReadOnlyDictionary<ProtocolCode, Type> packetTypes)
        {
            PacketTypes.Clear();
            foreach (var (code, Type) in packetTypes)
            {
                PacketTypes.Add(code, Type);
            }
        }

        public static PacketChannel Create(ITransport transport)
        {
            return new PacketChannel(transport);
        }
    }
}
