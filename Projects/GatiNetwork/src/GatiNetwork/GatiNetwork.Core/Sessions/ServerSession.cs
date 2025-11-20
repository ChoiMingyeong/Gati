using GatiNetwork.Core.Packets;
using GatiNetwork.Core.RecordStructs;
using GatiNetwork.Core.Transport;
using MemoryPack;
using Microsoft.Extensions.ObjectPool;
using System.Collections.Concurrent;
using System.Net;
using System.Net.WebSockets;
using System.Threading.Tasks.Dataflow;

namespace GatiNetwork.Core.Sessions
{
    public sealed class ServerSession : IServerSession
    {
        public ConcurrentDictionary<SessionID, ClientSession> Sessions { get; } = [];

        private readonly HttpListener _httpListener;
        private readonly ObjectPool<ClientSession> _pool;

        public ServerSession()
        {
            _httpListener = new();
            _pool = new DefaultObjectPool<ClientSession>(new DefaultPooledObjectPolicy<ClientSession>());
        }

        public Task StartAsync(ushort port, CancellationToken cancellationToken = default)
        {
            _httpListener.Prefixes.Add($"http://+:{port}/");
            _httpListener.Start();

            _ = Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var ctx = await _httpListener.GetContextAsync();

                    if (ctx.Request.IsWebSocketRequest && ctx.Request.Url?.AbsolutePath == "/ws")
                    {
                        _ = HandleWebSocketAsync(ctx);
                    }
                    else
                    {
                        ctx.Response.StatusCode = 404;
                        ctx.Response.Close();
                    }
                }
            }, cancellationToken);

            return Task.CompletedTask;
        }

        public void Stop()
        {
            _httpListener.Stop();
            _httpListener.Close();
        }

        private async Task HandleWebSocketAsync(HttpListenerContext ctx)
        {
            var wsContext = await ctx.AcceptWebSocketAsync(null);
            var webSocket = wsContext.WebSocket;

            var session = _pool.Get();
            session.Attach(SessionID.Create(), new WebSocketTransport(webSocket));

            Sessions[session.ID] = session;

            try
            {
                await session.StartReceiveLoopAsync();
            }
            finally
            {
                Sessions.TryRemove(session.ID, out _);
                session.Reset();
                _pool.Return(session);
            }

        }

        public async Task CloseAsync(WebSocketCloseStatus closeStatus, params SessionID[] sessionIDs)
        {

        }

        public async Task BroadcastAsync<TPacket>(TPacket packet, params SessionID[] sessionIDs) where TPacket : IPacket
        {

        }

        public async Task StopAsync()
        {
        }
    }
}
