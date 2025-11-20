using GatiNetwork.Core;
using GatiNetwork.Core.PacketHandlers;
using GatiNetwork.Core.Packets;
using GatiNetwork.Core.RecordStructs;
using GatiTest.Client.Handlers;
using GatiTest.Shared;
using MemoryPack;

namespace GatiTest.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            PacketHandlerRegistry.Initialize();
        }
    }

}
