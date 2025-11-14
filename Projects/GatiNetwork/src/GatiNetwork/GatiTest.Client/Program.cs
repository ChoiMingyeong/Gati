using GatiNetwork.Core;
using GatiTest.Shared;
using System.Reflection;

namespace GatiTest.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var map = await ProtocolCodeMapper.LoadPacketMappingsAsync(typeof(C2SProtocolCode).Assembly);
            
            RequestLogin requestLogin = new();

        }
    }

}
