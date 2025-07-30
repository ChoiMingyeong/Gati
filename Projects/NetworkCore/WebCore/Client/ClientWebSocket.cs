using MemoryPack;
using System.Net.WebSockets;
using WebCore.Shared.C2S;
using WebCore.Shared.S2C;

namespace WebCore.Client;
public class ClientWebSocket
{
    private System.Net.WebSockets.ClientWebSocket _socket = new();
    private Uri _uri;

    public async Task ConnectAsync(string uri)
    {
        _uri = new Uri(uri);
        await _socket.ConnectAsync(_uri, CancellationToken.None);
        Console.WriteLine("[클라이언트] 연결됨");

        _ = Task.Run(ReceiveLoop);
    }

    public async Task Send<TPacket>(TPacket packet) where TPacket : IPacket
    {
        var buffer = MemoryPackSerializer.Serialize(packet);
        var networkPacket = new NetworkPacket
        {
            Opcode = packet.GetOpcode(),
            Payload = buffer
        };

        await _socket.SendAsync(
            MemoryPackSerializer.Serialize(networkPacket),
            WebSocketMessageType.Binary,
            true,
            CancellationToken.None);
    }

    private async Task ReceiveLoop()
    {
        var buffer = new byte[2048];
        while (_socket.State == WebSocketState.Open)
        {
            try
            {
                var result = await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                    break;

                var rawData = new ReadOnlySpan<byte>(buffer, 0, result.Count);
                if (MemoryPackSerializer.Deserialize<NetworkPacket>(rawData) is NetworkPacket networkPacket)
                {
                    if (networkPacket.Opcode == WebCore.Shared.S2C.Opcode.RESPONSE_TEST)
                    {
                        if (MemoryPackSerializer.Deserialize<ResponseTest>(networkPacket.Payload) is ResponseTest response)
                        {
                            Console.WriteLine(response.ResponseCode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[에러] " + ex.Message);
            }
        }
    }
}