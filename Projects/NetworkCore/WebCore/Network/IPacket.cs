using MemoryPack;
using System.Reflection;
using WebCore.Shared;

namespace WebCore
{
    [MemoryPackable]
    public partial class IPacket
    {
        public virtual byte[] Serialize()
        {
            return MemoryPackSerializer.Serialize(this);
        }

        public virtual ushort GetOpcode()
        {
            return GetType().GetCustomAttribute<PacketAttribute>() is not PacketAttribute packetAttribute ?
                throw new NotImplementedException()
                : packetAttribute.Opcode;
        }
    }
}
