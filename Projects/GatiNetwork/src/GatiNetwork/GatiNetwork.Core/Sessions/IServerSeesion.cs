using GatiNetwork.Core.RecordStructs;
using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace GatiNetwork.Core.Sessions
{
    public interface IServerSeesion
    {
        ConcurrentDictionary<SessionID, IClientSession> Sessions { get; }

        Task StartAsync(string prefix, CancellationToken cancellationToken);

        Task StopAsync();

        Task CloseAsync(WebSocketCloseStatus closeStatus , params SessionID[] sessionIDs);

        Task BroadcastAsync<TPacket>(TPacket packet, params SessionID[] sessionIDs) where TPacket : IPacket;
    }
}
