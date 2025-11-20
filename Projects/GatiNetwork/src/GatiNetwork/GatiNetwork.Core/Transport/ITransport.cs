namespace GatiNetwork.Core.Transport
{
    public interface ITransport : IAsyncDisposable
    {
        static readonly byte ProtocolCodeSize = sizeof(ushort);

        bool IsConnected { get; }

        // 한 번에 하나의 프레임(메시지/데이터그램)을 전송
        Task SendAsync(ReadOnlyMemory<byte> frame, CancellationToken ct = default);

        // 프레임 스트림 (TCP면 length-framing 된 단위, UDP면 datagram, WS면 메시지)
        IAsyncEnumerable<ReadOnlyMemory<byte>> ReadFramesAsync(CancellationToken ct = default);

        Task CloseAsync(string? description = null,
                        Exception? error = null,
                        CancellationToken ct = default);
    }
}
