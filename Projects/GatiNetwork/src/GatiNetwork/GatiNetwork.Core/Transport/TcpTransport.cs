using GatiNetwork.Core.RecordStructs;
using GatiNetwork.Core.Sessions;
using System.Buffers.Binary;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace GatiNetwork.Core.Transport
{
    public sealed class TcpTransport(Socket socket) : ITransport
    {
        public bool IsConnected => _socket.Connected;
     
        private readonly Socket _socket = socket;

        public ValueTask DisposeAsync()
        {
            _socket.Dispose();
            return ValueTask.CompletedTask;
        }

        public async Task SendAsync(ReadOnlyMemory<byte> frame, CancellationToken ct = default)
        {
            ushort length = checked((ushort)frame.Length);
            byte[] buffer = new byte[2 + length];

            BinaryPrimitives.WriteUInt16LittleEndian(buffer, length);
            frame.CopyTo(buffer.AsMemory(2));

            using var ns = new NetworkStream(_socket, ownsSocket: false);
            await ns.WriteAsync(buffer, ct).ConfigureAwait(false);
        }

        public async IAsyncEnumerable<ReadOnlyMemory<byte>> ReadFramesAsync([EnumeratorCancellation] CancellationToken ct = default)
        {
            using var ns = new NetworkStream(_socket, ownsSocket: false);
            var lenBuf = new byte[2];

            while (!ct.IsCancellationRequested && _socket.Connected)
            {
                // 길이 읽기
                int read = await ns.ReadAsync(lenBuf.AsMemory(0, 2), ct).ConfigureAwait(false);
                if (read == 0) yield break;

                ushort length = BinaryPrimitives.ReadUInt16LittleEndian(lenBuf);
                var buffer = new byte[length];

                int offset = 0;
                while (offset < length)
                {
                    int r = await ns.ReadAsync(buffer.AsMemory(offset, length - offset), ct).ConfigureAwait(false);
                    if (r == 0) yield break;
                    offset += r;
                }

                yield return buffer;
            }
        }

        public Task CloseAsync(string? description = null, Exception? error = null, CancellationToken ct = default)
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
            return Task.CompletedTask;
        }
    }
}
