using GatiNetwork.Core.RecordStructs;
using System.Net.WebSockets;

namespace GatiNetwork.Core.Sessions
{
    public interface IClientSession
    {
        SessionID SessionID { get; }

        bool IsConnected { get; }

        void Connect(WebSocket socket);

        Task CloseAsync(WebSocketCloseStatus closeStatus, string? description = null);

        Task SendAsync<TPacket>(TPacket packet) where TPacket : IPacket;
    }
}
