using MemoryPack;
using System.Collections.Concurrent;
using System.Reflection;

namespace GatiNetwork.Core
{
    [MemoryPackable]
    public partial class IPacket
    {
        private static ConcurrentDictionary<Type, ushort> _cachedProtocolCode = [];

        public byte[] Serialize()
        {
            return MemoryPackSerializer.Serialize(GetType(), this);
        }

        public ushort GetProtocolCode()
        {
            _cachedProtocolCode.GetOrAdd(GetType(), type =>
            {
                return type.GetCustomAttribute<PacketProtocolAttribute>() is not PacketProtocolAttribute packetAttribute ?
                    throw new NotImplementedException()
                    : packetAttribute.ProtocolCode;
            });

            return GetType().GetCustomAttribute<PacketProtocolAttribute>() is not PacketProtocolAttribute packetAttribute ?
                throw new NotImplementedException()
                : packetAttribute.ProtocolCode;
        }
    }
}
