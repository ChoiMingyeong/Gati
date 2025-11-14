using GatiNetwork.Core.Packets;
using GatiNetwork.Core.RecordStructs;
using MemoryPack;
using Microsoft.Extensions.ObjectPool;
using System.Collections.Concurrent;
using System.Net;
using System.Net.WebSockets;
using System.Threading.Tasks.Dataflow;

namespace GatiNetwork.Core.Sessions
{
    public sealed class ServerSession : IServerSeesion
    {
        public ConcurrentDictionary<SessionID, IClientSession> Sessions { get; } = [];

        private readonly HttpListener _httpListener;
        private readonly BufferBlock<HttpListenerContext> _in;
        private readonly TransformBlock<HttpListenerContext, (HttpListenerContext, RequestConnect)> _parse;
        private readonly TransformBlock<(HttpListenerContext ctx, RequestConnect req), (HttpListenerContext ctx, ResponseConnect res)> _handle;
        private readonly TransformBlock<(HttpListenerContext ctx, ResponseConnect res), (HttpListenerContext ctx, byte[] body)> _serialize;
        private readonly ActionBlock<(HttpListenerContext ctx, byte[] body)> _respond;

        public ServerSession()
        {
            var pool = new DefaultObjectPool<ClientSession>(new ClientSessionPolicy());

            var exec = new ExecutionDataflowBlockOptions
            {
                EnsureOrdered = false,
                BoundedCapacity = 256
            };
            var link = new DataflowLinkOptions { PropagateCompletion = true };

            _httpListener = new();

            _in = new BufferBlock<HttpListenerContext>(new DataflowBlockOptions { BoundedCapacity = 256 });

            _parse = new TransformBlock<HttpListenerContext, (HttpListenerContext, RequestConnect)>(async ctx =>
            {
                if (ctx.Request.HttpMethod != "POST" || ctx.Request.Url?.AbsolutePath != "/connect")
                {
                    throw new HttpListenerException(404, "Not Found");
                }

                using var ms = new MemoryStream();
                await ctx.Request.InputStream.CopyToAsync(ms);
                var req = MemoryPackSerializer.Deserialize<RequestConnect>(ms.ToArray())
                          ?? throw new InvalidDataException("invalid payload");
                return (ctx, req);
            }, exec);

            _handle = new TransformBlock<(HttpListenerContext ctx, RequestConnect req), (HttpListenerContext, ResponseConnect)>(input =>
            {
                var session = pool.Get();
                Sessions[session.SessionID] = session;
                var packet = new ResponseConnect { SessionID = session.SessionID };
                return (input.ctx, packet);
            }, exec);

            _serialize = new TransformBlock<(HttpListenerContext ctx, ResponseConnect res), (HttpListenerContext, byte[])>(input =>
            {
                return (input.ctx, input.res.Serialize());
            }, exec);

            _respond = new ActionBlock<(HttpListenerContext ctx, byte[] body)>(async pair =>
            {
                var (ctx, body) = pair;
                ctx.Response.ContentType = "application/octet-stream";
                ctx.Response.ContentLength64 = body.Length;
                await ctx.Response.OutputStream.WriteAsync(body);
                ctx.Response.Close();
            }, exec);


            _in.LinkTo(_parse, link);
            _parse.LinkTo(_handle, link);
            _handle.LinkTo(_serialize, link);
            _serialize.LinkTo(_respond, link);
        }

        public Task StartAsync(string prefix, CancellationToken cancellationToken = default)
        {
            _httpListener.Prefixes.Add(prefix);
            _httpListener.Start();

            _ = Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var ctx = await _httpListener.GetContextAsync();
                    await _in.SendAsync(ctx, cancellationToken);
                }
            }, cancellationToken);

            return Task.CompletedTask;
        }

        public async Task StopAsync()
        {
            _in.Complete();
            await _respond.Completion;
            _httpListener.Stop();
            _httpListener.Close();
        }

        private async Task AcceptAsync()
        {
            HttpListenerContext context = await _httpListener.GetContextAsync();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            try
            {

            }
            catch (Exception ex)
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            finally
            {
                response.Close();
            }
        }

        public async Task CloseAsync(WebSocketCloseStatus closeStatus, params SessionID[] sessionIDs)
        {

        }

        public async Task BroadcastAsync<TPacket>(TPacket packet, params SessionID[] sessionIDs) where TPacket : IPacket
        {

        }
    }
}
