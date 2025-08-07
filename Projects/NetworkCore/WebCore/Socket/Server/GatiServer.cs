using System.Collections.Concurrent;
using System.Net;
using TSID.Creator.NET;
using WebCore.Packet;

namespace WebCore.Socket.Server
{
    public class GatiServer
    {
        private readonly HttpListener _listener = new();
        private readonly ConcurrentDictionary<Tsid, GatiSocket> _clients = new();
        private readonly ConcurrentQueue<NetworkPacket> _recvPackets = [];

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
            GatiSocket session = new(wsContext.WebSocket);
            Tsid sessionID;
            do { sessionID = TsidCreator.GetTsid(); }
            while (_clients.ContainsKey(sessionID));

            if (_clients.TryAdd(sessionID, session))
            {
                Console.WriteLine($"[클라이언트 접속] {sessionID}");
                await session.Connected();

                try
                {
                    while (false == session.ReceivedPackets.IsEmpty || session.IsConnected)
                    {
                        while (session.ReceivedPackets.TryDequeue(out var packet))
                        {
                            _recvPackets.Enqueue(packet);
                        }

                        await Task.Delay(10);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[에러] {sessionID} → {ex.Message}");
                }
                finally
                {
                    if(_clients.TryRemove(sessionID, out _))
                    {
                        await session.Disconnected();
                    }

                    Console.WriteLine($"[연결 종료] {sessionID}");
                }
            }
        }
    }

    //public abstract class GatiSocket<TPacketHandler> where TPacketHandler : IPacketHandler
    //{
    //    protected readonly TPacketHandler _packetHandler;
    //    protected readonly ConcurrentQueue<IPacket> _receivedPackets = [];

    //    public GatiSocket()
    //    {
    //        _packetHandler = Activator.CreateInstance<TPacketHandler>();
    //        _packetHandler.RegisterPacketHandlers();
    //    }

    //    private NetworkPacket EncodePacket<TPacket>([NotNull] TPacket packet) where TPacket : IPacket
    //    {
    //        return new NetworkPacket
    //        {
    //            Opcode = packet.GetOpcode(),
    //            Payload = packet.Serialize(),
    //        };
    //    }

    //    private TPacket? DecodePacket<TPacket>(in ReadOnlySpan<byte> buffer) where TPacket : IPacket
    //    {
    //        if (MemoryPackSerializer.Deserialize<NetworkPacket>(buffer) is not NetworkPacket networkPacket)
    //            return null;

    //        return MemoryPackSerializer.Deserialize<TPacket>(buffer);
    //    }

    //    public async Task SendAsync<TPacket>(WebSocket socket, [NotNull] TPacket packet) where TPacket : IPacket
    //    {
    //        await socket.SendAsync(
    //            MemoryPackSerializer.Serialize(EncodePacket(packet)),
    //            WebSocketMessageType.Binary,
    //            true,
    //            CancellationToken.None);
    //    }

    //    protected async Task ReceiveAsync(WebSocket socket)
    //    {
    //        byte[] buffer = ArrayPool<byte>.Shared.Rent(4096);
    //        try
    //        {
    //            while (socket.State == WebSocketState.Open)
    //            {
    //                var result = await socket.ReceiveAsync(buffer, CancellationToken.None);
    //                if (result.MessageType == WebSocketMessageType.Close)
    //                    break;

    //                var rawData = new ReadOnlySpan<byte>(buffer, 0, result.Count);
    //                if (MemoryPackSerializer.Deserialize<NetworkPacket>(rawData) is NetworkPacket networkPacket)
    //                {
    //                    await _packetHandler.RouteAsync(socket, networkPacket.Opcode, networkPacket.Payload);
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"[에러] {ex.Message}");
    //        }
    //        finally
    //        {
    //            ArrayPool<byte>.Shared.Return(buffer);
    //        }
    //    }
    //}
}
