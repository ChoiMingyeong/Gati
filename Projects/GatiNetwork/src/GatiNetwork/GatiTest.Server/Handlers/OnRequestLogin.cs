using GatiNetwork.Core.Sessions;
using GatiNetwork.Server;
using GatiTest.Shared;
using TSID.Creator.NET;

namespace GatiTest.Server.Handlers
{
    public class OnRequestLogin : PacketHandlerBase<RequestLogin>
    {
        public override async Task ExecuteAsync(ClientSession session, RequestLogin recPacket)
        {
            Console.WriteLine($"SessionID:{session.ID}, Email: {recPacket.Email}, Password: {recPacket.Password}");
            await session.SendAsync(new ResponseLogin()
            {
                UserID = TsidCreator.GetTsid(),
                Name = "Test",
            });
        }
    }
}
