using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameApi
{
    public class UserSession
    {
        public string ConnectionId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty; // 계정 ID
        public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;

    }

    public class SessionManager
    {
        private readonly ConcurrentDictionary<string, UserSession> _sessions = new(); // ConnectionId, UserSession
        private readonly ConcurrentDictionary<string, string> _userToConn = new(); // UserId, ConnectionId
        public bool Add(UserSession session) => _sessions.TryAdd(session.ConnectionId, session);

        public bool Remove(string connectionId) => _sessions.TryRemove(connectionId, out _);
        public UserSession? Get(string connectionId) => _sessions.TryGetValue(connectionId, out var session) ? session : null;
        public IEnumerable<UserSession> GetAll() => _sessions.Values;

        public void Touch(string connectionId)
        {
            if (_sessions.TryGetValue(connectionId, out var session))
            {
                session.LastActive = DateTime.UtcNow;
            }
        }

        public string? GetConnByUser(string userId)
        {
            return _userToConn.TryGetValue(userId, out var connId) ? connId : null;
        }

        public void BindUser(string userId, string connectionId)
        {
            _userToConn[userId] = connectionId;
            if (_sessions.TryGetValue(connectionId, out var session))
                session.UserId = userId;
        }

        public void UnbindUser(string userId, string connectionId)
        {
            _userToConn.TryGetValue(userId, out var current);
            if (current == connectionId)
                _userToConn.TryRemove(new KeyValuePair<string, string>(userId, connectionId));
        }
    }
}
