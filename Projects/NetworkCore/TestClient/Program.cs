using TestCommon.Shared.C2S;
using WebCore.Socket.Client;

namespace TestClient
{
    internal class Program
    {
        static async Task Main()
        {
            ClientSocket<ClientPacketHandler> client = new();

            await client.ConnectAsync("ws://localhost:8080/ws/");

            while (true)
            {
                Console.Write("입력 > ");
                var input = Console.ReadLine();

                //var msg = new ChatMessage { Message = input };
                //await client.SendPacketAsync(1001, msg);

                await client.Send(new RequestTest() { Message = input });
            }
        }
    }
}
