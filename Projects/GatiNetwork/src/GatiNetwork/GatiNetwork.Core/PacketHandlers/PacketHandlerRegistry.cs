using GatiNetwork.Core.Packets;
using GatiNetwork.Core.RecordStructs;
using System.Reflection;

namespace GatiNetwork.Core.PacketHandlers
{
    public static class PacketHandlerRegistry
    {
        private static readonly Dictionary<ProtocolCode, IPacketHandlerInvoker> _handlers = [];

        private static bool _initialized = false;

        public static void Initialize()
        {
            if (_initialized)
            {
                return;
            }
            _initialized = true;

            var assembly = Assembly.GetCallingAssembly();
            var handlerInterfaceType = typeof(IPacketHandler<>);

            foreach (var type in assembly.GetTypes())
            {
                if (type.IsAbstract || type.IsInterface)
                {
                    continue;
                }

                // IPacketHandler<TPacket> 를 구현하는가?
                var handlerInterfaces = type.GetInterfaces()
                    .Where(i => i.IsGenericType &&
                                i.GetGenericTypeDefinition() == handlerInterfaceType)
                    .ToArray();

                if (handlerInterfaces.Length == 0)
                {
                    continue;
                }

                foreach (var handlerInterface in handlerInterfaces)
                {
                    var packetType = handlerInterface.GetGenericArguments()[0];

                    // TPacket은 IPacket 이어야 함
                    if (!typeof(IPacket).IsAssignableFrom(packetType))
                    {
                        continue;
                    }

                    // 패킷 타입에서 ProtocolCode 추출 (PacketAttribute)
                    if (packetType.GetCustomAttribute<PacketProtocolAttribute>() is not PacketProtocolAttribute attr)
                    {
                        throw new InvalidOperationException($"Packet {packetType.Name}에 PacketAttribute가 없습니다.");
                    }

                    var protocol = attr.ProtocolCode;

                    // 핸들러 인스턴스 생성 (parameterless ctor 가정)
                    var handlerInstance = Activator.CreateInstance(type)
                                           ?? throw new InvalidOperationException($"Cannot create {type.FullName}");

                    // PacketHandlerInvoker<TPacket> 래퍼 생성
                    var invokerType = typeof(PacketHandlerInvoker<>).MakeGenericType(packetType);
                    var invoker = (IPacketHandlerInvoker)Activator.CreateInstance(invokerType, handlerInstance)!;

                    _handlers[protocol] = invoker;
                }
            }
        }
        
        public static bool TryGetHandler(ProtocolCode protocol, out IPacketHandlerInvoker? invoker)
            => _handlers.TryGetValue(protocol, out invoker);
    }
}
