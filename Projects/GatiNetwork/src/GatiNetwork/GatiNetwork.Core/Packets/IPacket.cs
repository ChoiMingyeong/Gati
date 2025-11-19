using GatiNetwork.Core.Packets;
using GatiNetwork.Core.RecordStructs;
using MemoryPack;
using System.Collections.Concurrent;
using System.Reflection;

namespace GatiNetwork.Core
{
    [MemoryPackable]
    public partial class IPacket
    {
        private static ConcurrentDictionary<Type, ProtocolCode> _cachedProtocolCode = [];

        public byte[] Serialize()
        {
            return MemoryPackSerializer.Serialize(GetType(), this);
        }

        public ProtocolCode GetProtocolCode()
        {
            var protocolCode = _cachedProtocolCode.GetOrAdd(GetType(), type =>
            {
                return type.GetCustomAttribute<PacketProtocolAttribute<IProtocolCodeGroup>>() is not PacketProtocolAttribute<IProtocolCodeGroup> packetAttribute ?
                    throw new NotImplementedException()
                    : packetAttribute.ProtocolCode;
            });

            return protocolCode;
        }
    }
}