using GatiNetwork.Core.PacketHandlers;
using GatiNetwork.Core.Sessions;
using GatiTest.Shared;

namespace GatiTest.Client.Handlers
{
    public class OnRequestLogin : IPacketHandler<RequestLogin>
    {
        public Task ExecuteAsync(ClientSession session, RequestLogin recPacket)
        {
            Console.WriteLine($"SessionID:{session.ID}, Test: {recPacket.Test}");
            return Task.CompletedTask;
        }
    }
}
