using MemoryPack;
using System.Buffers;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Net.WebSockets;
using WebCore.Shared.C2S;
using WebCore.Shared.S2C;

namespace WebCore.Common
{
    public abstract class GatiSocket
    {
        protected readonly ConcurrentQueue<IPacket> _receivedPackets = [];

        private NetworkPacket EncodePacket<TPacket>([NotNull] TPacket packet) where TPacket : IPacket
        {
            return new NetworkPacket
            {
                Opcode = packet.GetOpcode(),
                Payload = packet.Serialize(),
            };
        }

        private TPacket? DecodePacket<TPacket>(in ReadOnlySpan<byte> buffer) where TPacket : IPacket
        {
            if (MemoryPackSerializer.Deserialize<NetworkPacket>(buffer) is not NetworkPacket networkPacket)
                return null;

            return MemoryPackSerializer.Deserialize<TPacket>(buffer);
        }

        protected async Task SendAsync<TPacket>(WebSocket socket, [NotNull] TPacket packet) where TPacket : IPacket
        {
            await socket.SendAsync(
                MemoryPackSerializer.Serialize(EncodePacket(packet)),
                WebSocketMessageType.Binary,
                true,
                CancellationToken.None);
        }

        protected async Task ReceiveAsync(WebSocket socket)
        {
            byte[] buffer = ArrayPool<byte>.Shared.Rent(4096);
            try
            {
                while (socket.State == WebSocketState.Open)
                {
                    var result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close)
                        break;

                    var rawData = new ReadOnlySpan<byte>(buffer, 0, result.Count);
                    if (MemoryPackSerializer.Deserialize<NetworkPacket>(rawData) is NetworkPacket networkPacket)
                    {
                        Type packetType = typeof(IPacket);

                        switch(networkPacket.Opcode)
                        {
                            case Shared.S2C.Opcode.RESPONSE_TEST:
                                packetType = typeof(ResponseTest);
                                break;
                            case Shared.C2S.Opcode.REQUEST_TEST:
                                packetType = typeof(RequestTest);
                                break;
                        }

                        var packet = MemoryPackSerializer.Deserialize(packetType, networkPacket.Payload);
                        if (null != packet)
                        {
                            _receivedPackets.Enqueue((IPacket)packet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[에러] {ex.Message}");
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }
    }
}
