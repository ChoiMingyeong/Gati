using MemoryPack;
using System.Reflection;
using WebCore.Packet;

namespace WebCore.Socket.Server
{
    public abstract class IServerPacketRouter : IPacketRouter
    {
        public delegate Task ProtocolHandlerDelegate<TPacket>(ClientSession session, TPacket packet) where TPacket : IPacket;

        public Dictionary<ushort, Func<ClientSession, byte[], Task>> ProtocolMethods { get; }

        public IServerPacketRouter()
        {
            ProtocolMethods = [];
            RegisterProtocolMethods();
        }

        public void RegisterProtocolMethods()
        {
            var methods = GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(m => m.GetCustomAttribute<PacketHandlerAttribute>() != null);

            foreach (var method in methods)
            {
                var parameters = method.GetParameters();
                if (parameters.Length != 2 || parameters[0].ParameterType != typeof(ClientSession))
                {
                    continue;
                }

                var packetType = parameters[1].ParameterType;
                var packetAttribute = packetType.GetCustomAttribute<PacketProtocolAttribute>();
                if (packetAttribute == null)
                {
                    Console.WriteLine($"[오류] {packetType.Name}에 PacketAttribute가 없습니다.");
                    continue;
                }

                var delegateType = typeof(ProtocolHandlerDelegate<>).MakeGenericType(packetType);
                var del = Delegate.CreateDelegate(delegateType, this, method);

                var registerMethod = GetType().GetMethod(nameof(Register))!;
                var genericRegister = registerMethod.MakeGenericMethod(packetType);
                genericRegister.Invoke(this, [del]);
            }
        }

        public void Register<TPacket>(ProtocolHandlerDelegate<TPacket> handler) where TPacket : IPacket
        {
            var packetType = typeof(TPacket);
            if (packetType.GetCustomAttribute<PacketProtocolAttribute>() is not PacketProtocolAttribute packetAttribute)
            {
                throw new NotImplementedException($"PacketAttribute가 {packetType.Name}에 없습니다.");
            }

            ProtocolMethods[packetAttribute.Opcode] = async (session, raw) =>
            {
                // 역직렬화 후 핸들러 호출
                if (MemoryPackSerializer.Deserialize<TPacket>(raw) is TPacket packet)
                {
                    await handler(session, packet);
                }
                else
                {
                    Console.WriteLine($"[오류] 등록되지 않은 패킷: {packetAttribute.Opcode}");
                }
            };
        }
    }
}
