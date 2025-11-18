using GatiNetwork.Core.RecordStructs;
using System.Net.WebSockets;

namespace GatiNetwork.Core.Sessions
{
    public interface IClientSession
    {
        SessionID SessionID { get; }

        bool IsConnected { get; }

        void Reset();

        void OnConnected();

        Task OnClosedAsync();

        Task CloseAsync(WebSocketCloseStatus closeStatus, string? description = null);

        Task StartReceiveLoopAsync(CancellationToken ct = default);

        Task OnReceiveAsync(byte[] rawData);

        Task SendAsync<TPacket>(TPacket packet) where TPacket : IPacket;
    }
}
