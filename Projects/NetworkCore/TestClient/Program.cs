using GatiWeb.Core.Socket.Client;
using TestCommon.Shared.C2S;

namespace TestClient
{
    internal class Program
    {
        static async Task Main()
        {
            GatiClient<TestClientRouter> client = new();

            await client.ConnectAsync("ws://localhost:5060/ws/");

            while (true)
            {
                Console.Write("입력 > ");
                var input = Console.ReadLine();

                await client.SendAsync(new RequestTest() { Message = input ?? string.Empty });
            }
        }
    }
}
