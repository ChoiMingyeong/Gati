using GatiNetwork.Core.RecordStructs;

namespace GatiNetwork.Core.Sessions
{
    public interface IClientSession
    {
        /// <summary>
        /// 세션 고유 ID
        /// </summary>
        SessionID SessionID { get; }

        /// <summary>
        /// 현재 소켓 연결 여부
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// ObjectPool에서 재사용을 위해 내부 상태 초기화
        /// </summary>
        void Reset();

        /// <summary>
        /// 소켓 수신 루프를 시작<br/>
        /// 이 메서드는 루프가 끝날 때(연결 종료/오류)까지 반환되지 않음
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task StartReceiveLoopAsync(CancellationToken ct = default);

        /// <summary>
        /// 패킷 비동기 전송
        /// </summary>
        /// <typeparam name="TPacket"></typeparam>
        /// <param name="sendPacket"></param>
        /// <returns></returns>
        Task SendAsync<TPacket>(TPacket sendPacket) where TPacket : IPacket;

        /// <summary>
        /// 소켓 비동기 종료
        /// </summary>
        /// <param name="closeStatus"></param>
        /// <param name="description"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task CloseAsync(
            SessionCloseReason reason = SessionCloseReason.Normal,
            string? description = null,
            Exception? error = null,
            CancellationToken ct = default);

        public async Task OnReceiveAsync(byte[] rawData)
        {

        }

        #region 이벤트 영역
        /// <summary>
        /// 세션이 연결되었을 때 발생하는 이벤트
        /// </summary>
        event EventHandler<SessionConnectedEventArgs>? Connected;

        /// <summary>
        /// 세션이 정상/비정상적으로 종료되었을 때 발생하는 이벤트
        /// </summary>
        event EventHandler<SessionClosedEventArgs>? Closed;

        /// <summary>
        /// 패킷이 수신되었을 때 발생하는 이벤트
        /// </summary>
        event EventHandler<PacketReceiveEventArgs>? PacketReceive;

        /// <summary>
        /// 수신/송신/디코딩 과정에서 예외가 발생했을 때 발생하는 이벤트
        /// </summary>
        event EventHandler<SessionFaultedEventArgs>? Faulted;
        #endregion
    }
}
