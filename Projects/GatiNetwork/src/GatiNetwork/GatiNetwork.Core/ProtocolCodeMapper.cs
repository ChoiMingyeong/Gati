using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks.Dataflow;

namespace GatiNetwork.Core
{
    public sealed class ProtocolCodeMapper
    {
        public static async Task<Dictionary<ushort, Type>> LoadPacketMappingsAsync(Assembly assembly)
        {
            var packetType = typeof(IPacket);
            var dict = new ConcurrentDictionary<ushort, Type>();

            // 공통 실행 옵션(병렬/백프레셔)
            var exec = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount,
                BoundedCapacity = 1024,
                EnsureOrdered = false
            };
            var link = new DataflowLinkOptions { PropagateCompletion = true };

            var source = new BufferBlock<Type>(new DataflowBlockOptions { BoundedCapacity = 1024 });

            var toCode = new TransformBlock<Type, (ushort code, Type type)>(type =>
            {
                if(type.GetCustomAttribute<PacketProtocolAttribute>() is not PacketProtocolAttribute attribute)
                {
                    throw new NotImplementedException(
                        $"{type.FullName}에 PacketProtocolAttribute가 정의되어 있지 않습니다.");
                }

                return (attribute.ProtocolCode, type);
            }, exec);

            var register = new ActionBlock<(ushort code, Type type)>(pair =>
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

            return new Dictionary<ushort, Type>(dict);
        }
    }
}
