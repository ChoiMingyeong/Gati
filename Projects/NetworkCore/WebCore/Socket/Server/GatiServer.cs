using System.Collections.Concurrent;
using System.Net;
using System.Net.WebSockets;
using TSID.Creator.NET;
using WebCore.Packet;

namespace WebCore.Socket.Server
{
    public class GatiServer<TRouter> where TRouter : IServerPacketRouter
    {
        private readonly HttpListener _listener = new();
        private readonly ConcurrentDictionary<Tsid, ClientSession> _clients = new();
        private readonly ConcurrentQueue<NetworkPacket> _recvPackets = [];
        private readonly TRouter _router;

        public GatiServer()
        {
            if (Activator.CreateInstance<TRouter>() is TRouter router)
            {
                _router = router;
            }
            else
            {
                throw new InvalidOperationException($"Failed to create instance of {typeof(TRouter).Name}.");
            }
        }

        public async Task StartAsync(string prefix)
        {
            _listener.Prefixes.Add(prefix);
            _listener.Start();
            Console.WriteLine($"HttpListener Start: {prefix}");

            while (true)
            {
                var context = await _listener.GetContextAsync();

                if (context.Request.IsWebSocketRequest)
                {
                    _ = AcceptAsync(context);
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                }
            }
        }

        private async Task AcceptAsync(HttpListenerContext context)
        {
            var wsContext = await context.AcceptWebSocketAsync(null);
            ClientSession session = new(wsContext.WebSocket);

            if (_clients.TryAdd(session.ID, session))
            {
                Console.WriteLine($"[클라이언트 접속] {session.ID}");
                await session.Connected();

                try
                {
                    while (session.Routable)
                    {
                        await RouteAsync(session);
                        await Task.Delay(10);
                    }
                }
                catch (WebSocketException wsex)
                {
                    Console.WriteLine($"[WebSocket 에러] {session.ID} → {wsex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[에러] {session.ID} → {ex.Message}");
                }
                finally
                {
                    if (_clients.TryRemove(session.ID, out _))
                    {
                        await session.Disconnected();
                    }

                    Console.WriteLine($"[연결 종료] {session.ID}");
                }
            }
        }

        private async Task RouteAsync(ClientSession session)
        {
            foreach (var networkPacket in session.GetReceivedPackets())
            {
                if (_router.ProtocolMethods.TryGetValue(networkPacket.Opcode, out var method))
                {
                    await method(session, networkPacket.Payload);
                }
            }
        }
    }
}
