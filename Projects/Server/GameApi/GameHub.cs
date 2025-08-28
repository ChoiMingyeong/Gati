using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using ServerLib;

namespace GameApi
{
    public class GameHub : Hub
    {
        private static ConcurrentDictionary<string, UserSession> _sessions = new ConcurrentDictionary<string, UserSession>();

        public override async Task OnConnectedAsync()
        {
            var session = new UserSession
            {
                ConnectionId = Context.ConnectionId,
                UserId = Context.UserIdentifier ?? "Guest_" + Context.ConnectionId
            };

            _sessions.TryAdd(Context.ConnectionId, session);

            Logger.Default.LogDebug("[GameHub] Connected: {0} ({1})", session.UserId, session.ConnectionId);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _sessions.TryRemove(Context.ConnectionId, out var removed);
            Logger.Default.LogDebug("[GameHub] Disconnected: {0} ({1})", removed?.UserId, Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
