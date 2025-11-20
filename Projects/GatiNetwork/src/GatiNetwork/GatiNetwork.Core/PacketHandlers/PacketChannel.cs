using GatiNetwork.Core.RecordStructs;
using GatiNetwork.Core.Transport;
using MemoryPack;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace GatiNetwork.Core.PacketHandlers
{
    public sealed class PacketChannel(ITransport transport)
    {
        private const int ProtocolCodeSize = sizeof(ushort);

        private readonly ITransport _transport = transport;

        public async Task SendAsync<TPacket>(TPacket packet, CancellationToken ct = default)
            where TPacket : IPacket
        {
            var payload = MemoryPackSerializer.Serialize(packet);
            var protocolCode = packet.GetProtocolCode();
            var frame = new byte[ProtocolCodeSize + payload.Length];
            var span = frame.AsSpan();

            BinaryPrimitives.WriteUInt16LittleEndian(span, (ushort)protocolCode);
            payload.AsSpan().CopyTo(span[ProtocolCodeSize..]);

            await _transport.SendAsync(frame, ct).ConfigureAwait(false);
        }

        public async IAsyncEnumerable<IPacket> ReadPacketsAsync([EnumeratorCancellation] CancellationToken ct = default)
        {
            await foreach (var frame in _transport.ReadFramesAsync(ct).ConfigureAwait(false))
            {
                if (frame.Length < ProtocolCodeSize)
                {
                    continue;
                }

                var span = frame.Span;

                var protocolCode = (ProtocolCode)BinaryPrimitives.ReadUInt16LittleEndian(span);
                var payload = span[ProtocolCodeSize..];

                if (false == PacketChannelFactory.PacketTypes.TryGetValue(protocolCode, out var packetType))
                {
                    continue;
                }

                IPacket? packet = null;
                try
                {
                    packet = MemoryPackSerializer.Deserialize(packetType, payload) as IPacket;
                }
                catch
                {
                }

                if (packet is not null)
                {
                    yield return packet;
                }
            }
        }
    }
}
