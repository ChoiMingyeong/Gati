using System.Net.WebSockets;
using System.Reflection;

namespace WebCore.Packet
{
    public abstract class IPacketHandler
    {
        private PacketRouter _packetRouter = new();

        public void RegisterPacketHandlers()
        {
            var methods = GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(m => m.GetCustomAttribute<ProtocolMethodAttribute>() != null);

            foreach (var method in methods)
            {
                var parameters = method.GetParameters();
                if (parameters.Length != 2 || parameters[0].ParameterType != typeof(WebSocket))
                {
                    continue;
                }

                var packetType = parameters[1].ParameterType;
                var packetAttribute = packetType.GetCustomAttribute<PacketAttribute>();
                if (packetAttribute == null)
                {
                    Console.WriteLine($"[오류] {packetType.Name}에 PacketAttribute가 없습니다.");
                    continue;
                }

                var delegateType = typeof(PacketDelegate<>).MakeGenericType(packetType);
                var del = Delegate.CreateDelegate(delegateType, this, method);

                // Register<T>(ushort opcode, PacketHandler<T> handler) 호출
                var registerMethod = typeof(PacketRouter).GetMethod(nameof(PacketRouter.Register))!;
                var genericRegister = registerMethod.MakeGenericMethod(packetType);
                genericRegister.Invoke(_packetRouter, [del]);
            }
        }

        public async Task RouteAsync(WebSocket socket, ushort opcode, byte[] raw)
        {
            await _packetRouter.RouteAsync(socket, opcode, raw);
        }
    }
}
