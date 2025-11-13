using GatiNetwork.Core;
using MemoryPack;
using System.Reflection;

namespace GatiTest.Shared
{
    [MemoryPackable]
    [PacketProtocol(C2SProtocolCode.RequestLogin)]
    public partial class RequestLogin : IPacket
    {
        public RequestLogin()
        {
            var map = ProtocolCodeMapper.LoadPacketMappingsAsync(Assembly.GetExecutingAssembly()).GetAwaiter().GetResult();
            var testCode = GetProtocolCode();
        }
    }

}
