using GatiNetwork.Core;
using GatiNetwork.Core.Packets;
using GatiTest.Shared;
using MemoryPack;

namespace GatiTest.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var map = await ProtocolCodeMapper.LoadPacketMappingsAsync(typeof(C2SProtocolCode).Assembly);

            var packet = new RequestLogin();
            var serialize = MemoryPackSerializer.Serialize(packet);
            var deserialize = MemoryPackSerializer.Deserialize<IPacket>(serialize);
            var packett = MemoryPackSerializer.Deserialize<RequestLogin>(serialize);

        }
    }

}
