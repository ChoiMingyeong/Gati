using WebCore.Server;

namespace TestServer
{
    internal class Program
    {
        static async Task Main()
        {
            var server = new WebSocketServer();
            await server.StartAsync(); // 기본 포트 8080/ws/
        }
    }
}
