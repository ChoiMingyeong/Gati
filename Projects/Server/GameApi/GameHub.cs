using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using ServerLib;

namespace GameApi
{
    public class GameHub : Hub
    {
        private readonly SessionManager _sessionManager;
        public GameHub(SessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public override async Task OnConnectedAsync()
        {
            var session = new UserSession
            {
                ConnectionId = Context.ConnectionId,
                UserId = $"Guest_{Context.ConnectionId[..5]}"
            };

            _sessionManager.Add(session);
            Logger.Default.LogDebug("[GameHub] Connected: {0} ({1})", session.UserId, session.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            bool result = _sessionManager.Remove(Context.ConnectionId);
            Logger.Default.LogDebug("[GameHub] Disconnected: {0} ({1})", result, Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
