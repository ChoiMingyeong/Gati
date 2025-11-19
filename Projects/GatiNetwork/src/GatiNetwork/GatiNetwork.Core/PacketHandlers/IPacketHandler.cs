using GatiNetwork.Core.Sessions;
using System.Net;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace GatiNetwork.Core.PacketHandlers
{
    public interface IPacketHandler<TPacket> where TPacket : IPacket
    {
        Task Execute(IClientSession session, TPacket packet);
    }
}

public sealed class HttpDataflowServer
{
    private readonly HttpListener _listener = new();
    private readonly BufferBlock<HttpListenerContext> _ws;
    private readonly ActionBlock<HttpListenerContext> _res;

    public HttpDataflowServer(string prefix, int parallel = 8)
    {
        _listener.Prefixes.Add(prefix);

        var exec = new ExecutionDataflowBlockOptions
        {
            MaxDegreeOfParallelism = parallel,
            EnsureOrdered = false,
            BoundedCapacity = 256
        };
        var link = new DataflowLinkOptions { PropagateCompletion = true };
    }

    public async Task StartAsync(CancellationToken ct = default)
    {
        _listener.Start();

        // 수신 루프: 각 요청을 파이프라인에 밀어 넣음
        _ = Task.Run(async () =>
        {
            while (!ct.IsCancellationRequested)
            {
                var ctx = await _listener.GetContextAsync();

            }
        }, ct);
    }

    public async Task StopAsync()
    {
        _listener.Stop();
        _listener.Close();
    }
}