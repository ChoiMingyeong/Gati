using GatiNetwork.Client;
using GatiNetwork.Core.Sessions;
using GatiTest.Shared;

namespace GatiTest.Client.Handlers
{
    public sealed class OnResponseLogin : PacketHandlerBase<ResponseLogin>
    {
        public override Task ExecuteAsync(ServerSession session, ResponseLogin recPacket)
        {
            Console.WriteLine($"UserID: {recPacket.UserID}, Name: {recPacket.Name}");
            return Task.CompletedTask;
        }
    }
}
