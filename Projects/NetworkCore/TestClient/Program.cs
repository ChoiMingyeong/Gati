using WebCore.Client;
using WebCore.Shared;

namespace TestClient
{
    internal class Program
    {
        static async Task Main()
        {
            var client = new WebSocketClient();
            await client.ConnectAsync("ws://localhost:8080/ws/");

            while (true)
            {
                Console.Write("입력 > ");
                var input = Console.ReadLine();

                var msg = new ChatMessage { Message = input };
                await client.SendPacketAsync(1001, msg);
            }
        }
    }
}
