using System.Net.WebSockets;
using TestCommon.Shared.C2S;
using TestCommon.Shared.S2C;
using WebCore.Network;
using WebCore.Socket.Server;

namespace TestServer
{
    internal class Program
    {
        static ServerSocket? ServerSocket { get; set; } = null;

        static async Task Main()
        {
            PacketRouter router = new();
            router.On<RequestTest>(OnRequestTest);

            ServerSocket = new ServerSocket(router);

            await ServerSocket.StartAsync(); // 기본 포트 8080/ws/
        }

        public static async Task OnRequestTest(WebSocket socket, RequestTest packet)
        {
            Console.WriteLine(packet.Message);
            await ServerSocket!.SendAsync(socket, new ResponseTest());
        }
    }
}
