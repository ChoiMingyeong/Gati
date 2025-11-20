using GatiNetwork.Core.PacketHandlers;
using GatiNetwork.Core.Packets;
using GatiNetwork.Core.RecordStructs;
using GatiNetwork.Core.Sessions;
using MemoryPack;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks.Dataflow;

namespace GatiNetwork.Core
{
    public sealed class PacketDispatcher
    {
        private readonly BufferBlock<(ClientSession session, IPacket packet)> _in;
        private readonly ActionBlock<(ClientSession session, IPacket packet)> _handle;

        public ITargetBlock<(ClientSession session, IPacket packet)> Target => _in;

        public PacketDispatcher()
        {
            var exec = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount,
                EnsureOrdered = false,
                BoundedCapacity = 1024
            };

            _in = new BufferBlock<(ClientSession, IPacket)>(
                new DataflowBlockOptions { BoundedCapacity = 1024 });

            _handle = new ActionBlock<(ClientSession session, IPacket packet)>(DispatchAsync, exec);

            _in.LinkTo(_handle, new DataflowLinkOptions { PropagateCompletion = true });
        }

        private async Task DispatchAsync((ClientSession session, IPacket packet) input)
        {
            var (session, packet) = input;
            var protocol = packet.GetProtocolCode(); // IPacket 확장 메서드 or 구현

            if (!PacketHandlerRegistry.TryGetHandler(protocol, out var invoker) ||
                invoker is null)
            {
                // 핸들러 없음 → 로그만 찍고 무시 가능
                Console.WriteLine($"No handler for protocol: {protocol}");
                return;
            }

            try
            {
                await invoker.ExecuteAsync(session, packet).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // 핸들러 내부 예외 → 세션 Faulted 이벤트 or 로그
                Console.WriteLine($"Handler error for {protocol}: {ex}");
            }
        }

        public void Complete() => _in.Complete();
        public Task Completion => _handle.Completion;
    }

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
                return type.GetCustomAttribute<PacketProtocolAttribute>() is not PacketProtocolAttribute packetAttribute ?
                    throw new NotImplementedException()
                    : packetAttribute.ProtocolCode;
            });

            return protocolCode;
        }
    }
}