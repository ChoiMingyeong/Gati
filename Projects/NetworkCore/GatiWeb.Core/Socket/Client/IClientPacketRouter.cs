using GatiWeb.Core.Packet;
using MemoryPack;
using System.Reflection;

namespace GatiWeb.Core.Socket.Client
{
    public abstract class IClientPacketRouter : IPacketRouter
    {
        public delegate Task ProtocolHandlerDelegate<TPacket>(TPacket packet) where TPacket : IPacket;

        public GatiSocket Sender { get; }

        public Dictionary<ushort, Func<byte[], Task>> ProtocolMethods { get; }

        public IClientPacketRouter(GatiSocket sender)
        {
            Sender = sender;
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
                if (parameters.Length != 1)
                {
                    continue;
                }

                var packetType = parameters[0].ParameterType;
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

            ProtocolMethods[packetAttribute.Opcode] = async (raw) =>
            {
                // 역직렬화 후 핸들러 호출
                if (MemoryPackSerializer.Deserialize<TPacket>(raw) is TPacket packet)
                {
                    await handler(packet);
                }
            };
        }
    }
}
