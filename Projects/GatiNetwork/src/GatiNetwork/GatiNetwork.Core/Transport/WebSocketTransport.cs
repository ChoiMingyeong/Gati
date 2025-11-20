using System.Buffers;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;

namespace GatiNetwork.Core.Transport
{
    public sealed class WebSocketTransport(WebSocket socket) : ITransport
    {
        public bool IsConnected => _socket is { State: WebSocketState.Open or WebSocketState.CloseSent };

        private readonly WebSocket _socket = socket;

        public ValueTask DisposeAsync()
        {
            _socket.Dispose();
            return ValueTask.CompletedTask;
        }

        public async Task SendAsync(ReadOnlyMemory<byte> frame, CancellationToken ct = default)
        {
            await _socket.SendAsync(frame, WebSocketMessageType.Binary, endOfMessage: true, ct).ConfigureAwait(false);
        }

        public async IAsyncEnumerable<ReadOnlyMemory<byte>> ReadFramesAsync([EnumeratorCancellation] CancellationToken ct = default)
        {
            var buffer = ArrayPool<byte>.Shared.Rent(8192);
            try
            {
                using var ms = new MemoryStream();
                while(!ct.IsCancellationRequested && IsConnected)
                {
                    ms.SetLength(0);

                    WebSocketReceiveResult result;
                    do
                    {
                        var seg = new ArraySegment<byte>(buffer);
                        result = await _socket.ReceiveAsync(seg, ct).ConfigureAwait(false);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            yield break;
                        }

                        if (result.MessageType == WebSocketMessageType.Binary)
                        {
                            ms.Write(seg.Array!, seg.Offset, result.Count);
                        }
                    } while (!result.EndOfMessage);

                    yield return ms.ToArray();
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        public async Task CloseAsync(string? description = null, Exception? error = null, CancellationToken ct = default)
        {
            if (IsConnected)
            {
                await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, description,ct).ConfigureAwait(false);
            }
        }
    }
}
