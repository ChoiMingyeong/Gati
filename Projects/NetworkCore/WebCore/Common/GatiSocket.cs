using MemoryPack;
using System.Net.WebSockets;

namespace WebCore.Common
{
    public abstract class GatiWebSocket
    {
        private readonly Dictionary<ushort, Type> _packets = new();

        private NetworkPacket EncodePacket<TPacket>(TPacket packet) where TPacket : IPacket
        {
            return new NetworkPacket
            {
                Opcode = packet.GetOpcode(),
                Payload = packet.Serialize()
            };
        }

        private IPacket? DecodePacket(in ReadOnlySpan<byte> buffer)
        {
            if (MemoryPackSerializer.Deserialize<NetworkPacket>(buffer) is not NetworkPacket networkPacket)
                return null;


        }
    }
}
