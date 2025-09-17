using MemoryPack;
using System.Reflection;

namespace GatiWeb.Core.Packet
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
            return GetType().GetCustomAttribute<PacketProtocolAttribute>() is not PacketProtocolAttribute packetAttribute ?
                throw new NotImplementedException()
                : packetAttribute.Opcode;
        }
    }
}
