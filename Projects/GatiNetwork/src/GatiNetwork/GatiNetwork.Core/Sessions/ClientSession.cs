using GatiNetwork.Core.RecordStructs;
using MemoryPack;
using Microsoft.Extensions.ObjectPool;
using System.Net.WebSockets;
using TSID.Creator.NET;

namespace GatiNetwork.Core.Sessions
{
    public class ClientSessionPolicy : IPooledObjectPolicy<ClientSession>
    {
        public ClientSession Create()
        {
            return new ClientSession();
        }

        public bool Return(ClientSession obj)
        {
            if (obj.IsConnected is true)
            {
                return false;
            }

            obj.Reset();
            return true;
        }
    }

    public sealed class ClientSession : IClientSession
    {
        public SessionID SessionID { get; private set; }

        private WebSocket? _socket;

        public bool IsConnected => _socket is { State: WebSocketState.Open or WebSocketState.CloseSent };

        public ClientSession()
        {
            SessionID = TsidCreator.GetTsid();
            _socket = null;
        }

        public void Reset()
        {
            SessionID = TsidCreator.GetTsid();
            _socket?.Dispose();
            _socket = null;
        }

        public async Task SendAsync<TPacket>(TPacket packet) where TPacket : IPacket
        {
            if(_socket is null)
            {
                return;
            }

            if (false == IsConnected)
            {
                return;
            }

            await _socket.SendAsync(
                MemoryPackSerializer.Serialize(packet.Serialize()),
                WebSocketMessageType.Binary,
                true,
                CancellationToken.None);
        }

        public void Connect(WebSocket socket)
        {
            _socket = socket;
        }

        public async Task CloseAsync(WebSocketCloseStatus closeStatus, string? statusDescription = null)
        {
            if(_socket is null)
            {
                return;
            }

            await _socket.CloseAsync(
                closeStatus,
                statusDescription,
                CancellationToken.None);
        }
    }
}
