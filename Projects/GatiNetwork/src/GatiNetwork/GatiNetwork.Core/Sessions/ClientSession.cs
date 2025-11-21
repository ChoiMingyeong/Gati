using GatiNetwork.Core.PacketHandlers;
using GatiNetwork.Core.Packets;
using GatiNetwork.Core.RecordStructs;
using GatiNetwork.Core.Transport;

namespace GatiNetwork.Core.Sessions
{
    //public sealed class ClientSession : IClientSession
    //{
    //    public SessionID SessionID { get; private set; }

    //    private ITransport _transport;

    //    public event EventHandler<SessionConnectedEventArgs>? Connected;
    //    public event EventHandler<SessionClosedEventArgs>? Closed;
    //    public event EventHandler<PacketReceiveEventArgs>? PacketReceive;
    //    public event EventHandler<SessionFaultedEventArgs>? Faulted;

    //    public bool IsConnected => _socket is { State: WebSocketState.Open or WebSocketState.CloseSent };

    //    public ClientSession()
    //    {
    //        _socket = null;
    //    }

    //    public void Attach(SessionID sessionID, ITransport transport)
    //    {
    //        SessionID = SessionID.Create();
    //        _transport = transport;
    //    }

    //    public void Reset()
    //    {
    //        if (IsConnected)
    //        {
    //            _socket?.CloseAsync(WebSocketCloseStatus.NormalClosure, "Session Reset", CancellationToken.None).Wait();
    //            _socket?.Dispose();
    //        }

    //        SessionID = default;
    //        _socket = null;
    //    }

    //    public async Task StartReceiveLoopAsync(CancellationToken ct = default)
    //    {
    //        var pool = ArrayPool<byte>.Shared;
    //        // 8KB 기본 버퍼 (필요하면 더 크게 조정 가능)
    //        byte[] buffer = pool.Rent(8192);

    //        try
    //        {
    //            // 메시지가 여러 프래그먼트로 쪼개져서 올 때를 대비한 버퍼
    //            using var messageStream = new MemoryStream();

    //            while (!ct.IsCancellationRequested && IsConnected)
    //            {
    //                messageStream.SetLength(0);

    //                WebSocketReceiveResult result;
    //                do
    //                {
    //                    var segment = new ArraySegment<byte>(buffer);
    //                    result = await _socket!.ReceiveAsync(segment, ct).ConfigureAwait(false);

    //                    if (result.MessageType == WebSocketMessageType.Close)
    //                    {
    //                        // 클라이언트가 종료를 요청함
    //                        await CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None).ConfigureAwait(false);
    //                        return;
    //                    }

    //                    if (result.MessageType != WebSocketMessageType.Binary)
    //                    {
    //                        // 텍스트 메시지는 사용하지 않으니 무시하거나 로그만 남김
    //                        continue;
    //                    }

    //                    // 프래그먼트 누적
    //                    messageStream.Write(buffer, 0, result.Count);

    //                } while (!result.EndOfMessage);

    //                var raw = messageStream.ToArray();
    //                await OnReceiveAsync(raw).ConfigureAwait(false);
    //            }
    //        }
    //        catch (OperationCanceledException)
    //        {
    //            // cancel 정상 종료
    //        }
    //        catch (WebSocketException)
    //        {
    //            // 네트워크 오류 등: 로그 찍고 종료
    //        }
    //        finally
    //        {
    //            pool.Return(buffer);

    //            try
    //            {
    //                if (IsConnected)
    //                {
    //                    await CloseAsync(WebSocketCloseStatus.NormalClosure, "Server shutting down").ConfigureAwait(false);
    //                }
    //            }
    //            catch
    //            {
    //                // 이미 닫혔으면 무시
    //            }

    //            await OnClosedAsync().ConfigureAwait(false);
    //        }
    //    }

    //    private async Task OnReceiveAsync(byte[] rawData)
    //    {

    //    }

    //    public async Task SendAsync<TPacket>(TPacket sendPacket) where TPacket : IPacket
    //    {
    //        if (_socket is null)
    //        {
    //            return;
    //        }

    //        if (false == IsConnected)
    //        {
    //            return;
    //        }

    //        await _socket.SendAsync(
    //            MemoryPackSerializer.Serialize(sendPacket),
    //            WebSocketMessageType.Binary,
    //            true,
    //            CancellationToken.None);
    //    }

    //    public async Task CloseAsync(WebSocketCloseStatus closeStatus, string? statusDescription = null, CancellationToken ct = default)
    //    {
    //        if (_socket is null)
    //        {
    //            return;
    //        }

    //        await _socket.CloseAsync(closeStatus, statusDescription, ct);
    //    }

    //    public void OnConnected()
    //    {

    //    }

    //    public async Task OnClosedAsync()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task CloseAsync(SessionCloseReason reason = SessionCloseReason.Normal, string? description = null, Exception? error = null, CancellationToken ct = default)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public sealed class ClientSession : ISession
    {
        /// <summary>
        /// 세션 고유 ID
        /// </summary>
        public SessionID ID { get; private set; }


        /// <summary>
        /// 현재 소켓 연결 여부
        /// </summary>
        public bool IsConnected => _transport is { IsConnected: true };

        private ITransport? _transport = null;
        private PacketChannel? _channel = null;

        /// <summary>
        /// ObjectPool에서 재사용을 위해 내부 상태 초기화
        /// </summary>
        public void Reset()
        {
            ID = default;
            _transport = null;
            _channel = null;
        }

        public void Attach(SessionID sessionID, ITransport transport)
        {
            ID = sessionID;
            _transport = transport;
            _channel = PacketChannelFactory.Create(transport);

            Connected?.Invoke(this, new SessionConnectedEventArgs(ID));
        }

        /// <summary>
        /// 소켓 수신 루프를 시작<br/>
        /// 이 메서드는 루프가 끝날 때(연결 종료/오류)까지 반환되지 않음
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task StartReceiveLoopAsync(CancellationToken ct = default)
        {
            if (_transport is null || _channel is null)
            {
                throw new InvalidOperationException("Transport is not attached.");
            }

            while (!ct.IsCancellationRequested && IsConnected)
            {
                try
                {
                    await foreach (var packet in _channel.ReadPacketsAsync(ct).ConfigureAwait(false))
                    {
                        RaisePacketReceived(packet);
                    }
                }
                catch (OperationCanceledException)
                {
                    RaiseClosed(SessionCloseReason.Timeout, "Canceled", null);
                }
                catch (Exception ex)
                {
                    RaiseFaulted(ex);
                    RaiseClosed(SessionCloseReason.TransportError, ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// 패킷 비동기 전송
        /// </summary>
        /// <typeparam name="TPacket"></typeparam>
        /// <param name="sendPacket"></param>
        /// <returns></returns>
        public async Task SendAsync<TPacket>(TPacket sendPacket, CancellationToken ct = default) where TPacket : IPacket
        {
            if (_channel is null)
            {
                throw new InvalidOperationException("Transport is not attached.");
            }

            await _channel.SendAsync(sendPacket, ct);
        }

        /// <summary>
        /// 소켓 비동기 종료
        /// </summary>
        /// <param name="closeStatus"></param>
        /// <param name="description"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task CloseAsync(
            SessionCloseReason reason = SessionCloseReason.Normal,
            string? description = null,
            Exception? error = null,
            CancellationToken ct = default)
        {
            if (_transport is null)
            {
                return;
            }

            await _transport.CloseAsync(description, error, ct);
        }

        #region 이벤트 영역
        /// <summary>
        /// 세션이 연결되었을 때 발생하는 이벤트
        /// </summary>
        public event EventHandler<SessionConnectedEventArgs>? Connected;
        private void OnConnected()
            => Connected?.Invoke(this, new SessionConnectedEventArgs(ID));

        /// <summary>
        /// 세션이 정상/비정상적으로 종료되었을 때 발생하는 이벤트
        /// </summary>
        public event EventHandler<SessionClosedEventArgs>? Closed;
        private void RaiseClosed(SessionCloseReason reason, string? description, Exception? error)
            => Closed?.Invoke(this, new SessionClosedEventArgs(ID, reason, description, error));

        /// <summary>
        /// 패킷이 수신되었을 때 발생하는 이벤트
        /// </summary>
        public event EventHandler<PacketReceiveEventArgs>? PacketReceived;
        private void RaisePacketReceived(IPacket packet)
            => PacketReceived?.Invoke(this, new PacketReceiveEventArgs(ID, packet));

        /// <summary>
        /// 수신/송신/디코딩 과정에서 예외가 발생했을 때 발생하는 이벤트
        /// </summary>
        public event EventHandler<SessionFaultedEventArgs>? Faulted;
        private void RaiseFaulted(Exception error)
            => Faulted?.Invoke(this, new SessionFaultedEventArgs(ID, error));
        #endregion
    }
}
