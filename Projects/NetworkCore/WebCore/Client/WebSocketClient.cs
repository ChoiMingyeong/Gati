using System.Net.WebSockets;
using WebCore.Shared;

namespace WebCore.Client;
public class WebSocketClient
{
    private ClientWebSocket _socket = new();
    private Uri _uri;

    public async Task ConnectAsync(string uri)
    {
        _uri = new Uri(uri);
        await _socket.ConnectAsync(_uri, CancellationToken.None);
        Console.WriteLine("[클라이언트] 연결됨");

        _ = Task.Run(ReceiveLoop);
    }

    public async Task SendPacketAsync<T>(ushort opcode, T message)
    {
        byte[] payload = MemoryPack.MemoryPackSerializer.Serialize(message);
        var packet = new Packet { Opcode = opcode, Payload = payload };
        byte[] data = MemoryPack.MemoryPackSerializer.Serialize(packet);

        await _socket.SendAsync(
            new ArraySegment<byte>(data),
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

                var packet = MemoryPack.MemoryPackSerializer.Deserialize<Packet>(buffer.AsSpan(0, result.Count));
                Console.WriteLine($"[수신] opcode: {packet.Opcode}, payload length: {packet.Payload.Length}");

                // 여기에서 payload를 역직렬화할 수 있음
                if (packet.Opcode == 1001)
                {
                    var msg = MemoryPack.MemoryPackSerializer.Deserialize<ChatMessage>(packet.Payload);
                    Console.WriteLine("[채팅] " + msg.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[에러] " + ex.Message);
            }
        }
    }
}