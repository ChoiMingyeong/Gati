namespace GatiTest.Server
{

    internal class Program
    {
        static void Main(string[] args)
        {
            GatiTestServer server = new();

            server.Run();
        }
    }
}
