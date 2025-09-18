using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using ServerLib;
using ServerLib.Model;
using CommonLib;

namespace GameApi
{
    public partial class GameHub : Hub
    {
        private readonly SessionManager _sessionManager;
        private readonly DbService _db;
        
        public GameHub(SessionManager sessionManager, DbService db)
        {
            _sessionManager = sessionManager;
            _db = db;
        }

        public override async Task OnConnectedAsync()
        {
            var session = new UserSession
            {
                ConnectionId = Context.ConnectionId,
                UserId = $"Guest_{Context.ConnectionId[..5]}"
            };

            _sessionManager.Add(session);
            // 중복 접속할 경우 이전 연결에 강제 로그아웃 통지
            var prev = _sessionManager.GetConnByUser(session.UserId);
            if (prev != null && prev != Context.ConnectionId)
            {
                await Clients.Client(prev).SendAsync("ForceLogout");
            }
            _sessionManager.BindUser(session.UserId, Context.ConnectionId);
            await base.OnConnectedAsync();
            Logger.Default.LogTrace("[GameHub] Connected: {0} ({1})", session.UserId, session.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var s = _sessionManager.Get(Context.ConnectionId);
            if (s != null)
            {
                _sessionManager.UnbindUser(s.UserId, Context.ConnectionId);
            }
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
            Logger.Default.LogTrace("[GameHub] Login {0} {1} ", info.ConnectionId, info.UserEmail);

            // DB에서 사용자 정보 조회
            var auth = await _db.Auth.GetAuthByEmail(info.UserEmail);
            if (auth == null)
            {
                // 사용자가 없으면 기본 정보로 응답
                return new Response.UserInfo
                {
                    Name = "Guest",
                    Level = 0
                };
            }

            var account = await _db.Game.GetAccountByUserId(auth.UserId);
            if (account == null)
            {
                return null; // TODO : 값 리턴할때 성공실패 status도 전송하게 수정
            }

            return new Response.UserInfo
            {
                Name = account.UserName,
                Level = 1 // 실제로는 게임 데이터에서 가져와야 함
            };
        }
    }
}
