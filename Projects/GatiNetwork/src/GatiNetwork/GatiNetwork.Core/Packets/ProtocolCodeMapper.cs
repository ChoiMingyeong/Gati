using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks.Dataflow;

namespace GatiNetwork.Core.Packets
{
    public interface IPacketTypeRegistry
    {
        void Register(int code, Type packetType);
    }

    public sealed class PacketTypeRegistry : IPacketTypeRegistry
    {
        private readonly Dictionary<int, Type> _codeToType = new();

        public void Register(int code, Type packetType)
            => _codeToType[code] = packetType;

        public Type? GetPacketType(int code)
            => _codeToType.TryGetValue(code, out var t) ? t : null;
    }

    public sealed class ProtocolCodeMapper
    {
        public static async Task<Dictionary<int, Type>> LoadPacketMappingsAsync(Assembly assembly)
        {
            var packetType = typeof(IPacket);
            var dict = new ConcurrentDictionary<int, Type>();

            // 공통 실행 옵션(병렬/백프레셔)
            var exec = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount,
                BoundedCapacity = 1024,
                EnsureOrdered = false
            };
            var link = new DataflowLinkOptions { PropagateCompletion = true };

            var source = new BufferBlock<Type>(new DataflowBlockOptions { BoundedCapacity = 1024 });

            var toCode = new TransformBlock<Type, (int code, Type type)>(type =>
            {
                if (type.GetCustomAttribute<PacketProtocolAttribute>() is not PacketProtocolAttribute attribute)
                {
                    throw new NotImplementedException(
                        $"{type.FullName}에 PacketProtocolAttribute가 정의되어 있지 않습니다.");
                }

                return ((int)attribute.ProtocolCode, type);
            }, exec);

            var register = new ActionBlock<(int code, Type type)>(pair =>
            {
                if (!dict.TryAdd(pair.code, pair.type))
                {
                    var existing = dict[pair.code];
                    throw new InvalidOperationException(
                        $"중복 ProtocolCode {pair.code} : {existing.FullName} ↔ {pair.type.FullName}");
                }
            }, exec);

            source.LinkTo(toCode, link);
            toCode.LinkTo(register, link);

            foreach (var t in assembly.GetTypes())
            {
                if (packetType.IsAssignableFrom(t)
                    && t.IsClass
                    && !t.IsAbstract
                    && t != packetType)
                {
                    await source.SendAsync(t);
                }
            }

            source.Complete();
            await register.Completion;

            return new Dictionary<int, Type>(dict);
        }
    }
}
