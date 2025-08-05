using WebCore.Socket.Server;

namespace TestServer
{
    internal class Program
    {
        static async Task Main()
        {
            ServerSocket<ServerPacketHandler> server = new();
            await server.StartAsync();
        }
    }
}
