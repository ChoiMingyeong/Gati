using WebCore.Socket.Server;

namespace TestServer
{
    internal class Program
    {
        static async Task Main()
        {
            GatiServer server = new();
            await server.StartAsync("http://localhost:8080/ws/");
        }
    }
}
