using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using ServerLib;
using CommonLib;

namespace GameApi
{
    public partial class GameHub : Hub
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
            await base.OnConnectedAsync();
            Logger.Default.LogTrace("[GameHub] Connected: {0} ({1})", session.UserId, session.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            bool result = _sessionManager.Remove(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
            Logger.Default.LogTrace("[GameHub] Disconnected: {0} ({1})", result, Context.ConnectionId);
        }

        public Task Ping()
        {
            _sessionManager.Touch(Context.ConnectionId);    // 갱신
            return Task.CompletedTask;
        }

        public async Task<Response.UserInfo> Login(Request.LoginInfo info)
        {
            Logger.Default.LogTrace("[GameHub] Login {0} ", info.Id);

            return new Response.UserInfo
            {
                Name = "testName",
                Level = 0
            };
        }
    }
}
