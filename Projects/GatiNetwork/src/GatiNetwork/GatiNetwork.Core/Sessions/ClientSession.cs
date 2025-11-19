using GatiNetwork.Core.RecordStructs;
using MemoryPack;
using System.Buffers;
using System.Net.WebSockets;
using TSID.Creator.NET;

namespace GatiNetwork.Core.Sessions
{
    public sealed class ClientSession : IClientSession
    {
        public SessionID SessionID { get; private set; }

        private WebSocket? _socket;

        public event EventHandler<SessionConnectedEventArgs>? Connected;
        public event EventHandler<SessionClosedEventArgs>? Closed;
        public event EventHandler<PacketReceiveEventArgs>? PacketReceive;
        public event EventHandler<SessionFaultedEventArgs>? Faulted;

        public bool IsConnected => _socket is { State: WebSocketState.Open or WebSocketState.CloseSent };

        public ClientSession()
        {
            _socket = null;
        }

        public void Attach(WebSocket socket)
        {
            SessionID = SessionID.Create();
            _socket = socket;
        }

        public void Reset()
        {
            if (IsConnected)
            {
                _socket?.CloseAsync(WebSocketCloseStatus.NormalClosure, "Session Reset", CancellationToken.None).Wait();
                _socket?.Dispose();
            }

            SessionID = default;
            _socket = null;
        }

        public async Task StartReceiveLoopAsync(CancellationToken ct = default)
        {
            var pool = ArrayPool<byte>.Shared;
            // 8KB 기본 버퍼 (필요하면 더 크게 조정 가능)
            byte[] buffer = pool.Rent(8192);

            try
            {
                // 메시지가 여러 프래그먼트로 쪼개져서 올 때를 대비한 버퍼
                using var messageStream = new MemoryStream();

                while (!ct.IsCancellationRequested && IsConnected)
                {
                    messageStream.SetLength(0);

                    WebSocketReceiveResult result;
                    do
                    {
                        var segment = new ArraySegment<byte>(buffer);
                        result = await _socket!.ReceiveAsync(segment, ct).ConfigureAwait(false);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            // 클라이언트가 종료를 요청함
                            await CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None).ConfigureAwait(false);
                            return;
                        }

                        if (result.MessageType != WebSocketMessageType.Binary)
                        {
                            // 텍스트 메시지는 사용하지 않으니 무시하거나 로그만 남김
                            continue;
                        }

                        // 프래그먼트 누적
                        messageStream.Write(buffer, 0, result.Count);

                    } while (!result.EndOfMessage);

                    var raw = messageStream.ToArray();
                    await OnReceiveAsync(raw).ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException)
            {
                // cancel 정상 종료
            }
            catch (WebSocketException)
            {
                // 네트워크 오류 등: 로그 찍고 종료
            }
            finally
            {
                pool.Return(buffer);

                try
                {
                    if (IsConnected)
                    {
                        await CloseAsync(WebSocketCloseStatus.NormalClosure, "Server shutting down").ConfigureAwait(false);
                    }
                }
                catch
                {
                    // 이미 닫혔으면 무시
                }

                await OnClosedAsync().ConfigureAwait(false);
            }
        }

        private async Task OnReceiveAsync(byte[] rawData)
        {

        }

        public async Task SendAsync<TPacket>(TPacket sendPacket) where TPacket : IPacket
        {
            if (_socket is null)
            {
                return;
            }

            if (false == IsConnected)
            {
                return;
            }

            await _socket.SendAsync(
                MemoryPackSerializer.Serialize(sendPacket),
                WebSocketMessageType.Binary,
                true,
                CancellationToken.None);
        }

        public async Task CloseAsync(WebSocketCloseStatus closeStatus, string? statusDescription = null, CancellationToken ct = default)
        {
            if (_socket is null)
            {
                return;
            }

            await _socket.CloseAsync(closeStatus, statusDescription, ct);
        }

        public void OnConnected()
        {

        }

        public async Task OnClosedAsync()
        {
            throw new NotImplementedException();
        }

        public Task CloseAsync(SessionCloseReason reason = SessionCloseReason.Normal, string? description = null, Exception? error = null, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
