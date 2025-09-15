using GameApi;
using Microsoft.AspNetCore.Mvc;
using ServerLib;
using System.Linq;

namespace ProjectLyn
{
    [ApiController]
    [Route("gs/[action]")]
    public class GameServerController : ControllerBase
    {
        private readonly SessionManager _sessions;
        public GameServerController(SessionManager sessions)
        {
            _sessions = sessions;
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
    }
}
