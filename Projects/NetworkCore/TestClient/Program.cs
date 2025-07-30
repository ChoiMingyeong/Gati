using WebCore.Client;
using WebCore.Shared;
using WebCore.Shared.C2S;

namespace TestClient
{
    internal class Program
    {
        static async Task Main()
        {
            var client = new ClientWebSocket();
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
