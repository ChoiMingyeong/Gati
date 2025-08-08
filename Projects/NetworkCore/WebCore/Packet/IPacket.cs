using MemoryPack;
using System.Reflection;

namespace WebCore.Packet
{
    [MemoryPackable]
    public partial class IPacket
    {
        public virtual byte[] Serialize()
        {
            return MemoryPackSerializer.Serialize(GetType(), this);
        }

        public virtual ushort GetOpcode()
        {
            return GetType().GetCustomAttribute<PacketAttribute>() is not PacketAttribute packetAttribute ?
                throw new NotImplementedException()
                : packetAttribute.Opcode;
        }
    }
}
