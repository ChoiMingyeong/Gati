using GameApi;
using Microsoft.AspNetCore.Mvc;
using ServerLib;
using ServerLib.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectLyn
{
    [ApiController]
    [Route("gs/[action]")]
    public class GameServerController : ControllerBase
    {
        private readonly SessionManager _sessions;
        private readonly AuthContext _auth;
        public GameServerController(SessionManager sessions, AuthContext auth)
        {
            _sessions = sessions;
            _auth = auth;
        }

        [HttpGet]
        public IActionResult GetUserCount()
        {
            var serverInfo = ServerUtil.GetServerInfo();
            serverInfo.UserCount = _sessions.GetAll().Count();
            return Ok(new { count = serverInfo.UserCount });
        }

        [HttpGet]
        public IActionResult GetUserList()
        {
            return Ok(_sessions.GetAll().Select(s => new
            {
                s.UserId,
                s.ConnectionId,
                s.ConnectedAt,
                s.LastActive
            }));
        }

        [HttpGet]
        public IActionResult GetServerHeartbeat()
        {
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> DbHealth()
        {
            var canConnect = await _auth.Database.CanConnectAsync();
            int accountCount = 0;
            if (canConnect)
            {
                accountCount = await _auth.Set<account>().CountAsync();
            }
            return Ok(new { canConnect, accountCount });
        }
    }
}
