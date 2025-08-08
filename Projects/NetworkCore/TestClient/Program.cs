using TestCommon.Shared.C2S;
using WebCore.Socket.Client;

namespace TestClient
{
    internal class Program
    {
        static async Task Main()
        {
            GatiClient client = new();

            await client.ConnectAsync("ws://localhost:8080/ws/");

            while (true)
            {
                Console.Write("입력 > ");
                var input = Console.ReadLine();

                await client.SendAsync(new RequestTest() { Message = input });
            }
        }
    }
}
