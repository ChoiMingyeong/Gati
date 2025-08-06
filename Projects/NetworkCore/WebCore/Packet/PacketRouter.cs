using MemoryPack;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Reflection;

namespace WebCore.Packet
{
    internal delegate Task PacketDelegate<TSocket, TPacket>(TSocket socket, TPacket packet)
        where TSocket : WebSocket
        where TPacket : IPacket;

    internal class PacketRouter<TSocket> where TSocket : WebSocket
    {
        private static readonly ConcurrentDictionary<Type, ushort> _packetOpcodes = [];
        private readonly Dictionary<ushort, Func<TSocket, byte[], Task>> _handlers = [];

        // 핸들러 등록
        public void Register<TPacket>(PacketDelegate<TSocket, TPacket> handler) where TPacket : IPacket
        {
            var opcode = _packetOpcodes.GetOrAdd(typeof(TPacket), TPacket =>
            {
                // 패킷 타입에서 Opcode를 가져옴
                if(typeof(TPacket).GetCustomAttribute<PacketAttribute>() is PacketAttribute packetAttribute)
                {
                    return packetAttribute.Opcode;
                }

                throw new InvalidOperationException($"패킷 타입 {typeof(TPacket).Name}에 PacketAttribute가 없습니다.");
            });

            _handlers[opcode] = async (socket, raw) =>
            {
                // 역직렬화 후 핸들러 호출
                if(MemoryPackSerializer.Deserialize<TPacket>(raw) is TPacket packet)
                {
                    await handler(socket, packet);
                }
            };
        }

        // 패킷 라우팅
        public async Task RouteAsync(TSocket socket, ushort opcode, byte[] raw)
        {
            if (_handlers.TryGetValue(opcode, out var handler))
            {
                await handler(socket, raw);
            }
            else
            {
                Console.WriteLine($"[오류] 등록되지 않은 패킷: {opcode}");
            }
        }
    }
}
