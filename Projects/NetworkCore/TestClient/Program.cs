using System.Net.WebSockets;
using WebCore.Network;
using WebCore.Shared.C2S;
using WebCore.Shared.S2C;
using WebCore.Socket.Client;

namespace TestClient
{
    internal class Program
    {
        static async Task Main()
        {
            PacketRouter router = new();
            router.On<ResponseTest>(OnResponseTest);
            
            var client = new ClientSocket(router);

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
        public static async Task OnResponseTest(WebSocket socket, ResponseTest packet)
        {
            Console.WriteLine(packet.ResponseCode);
        }
    }
}
