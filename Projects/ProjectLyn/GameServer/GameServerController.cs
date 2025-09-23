using GameApi;
using Microsoft.AspNetCore.Mvc;
using ServerLib;
using ServerLib.Model;
using Microsoft.EntityFrameworkCore;
using System;
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
        private readonly UserContext _game;
        private readonly RedisService _redis;
        public GameServerController(SessionManager sessions, AuthContext auth, UserContext game, RedisService redis)
        {
            _sessions = sessions;
            _auth = auth;
            _game = game;
            _redis = redis;
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
            var authCanConnect = await _auth.Database.CanConnectAsync();
            var gameCanConnect = await _game.Database.CanConnectAsync();
            
            int authCount = 0;
            int gameCount = 0;
            
            if (authCanConnect)
            {
                authCount = await _auth.Set<auth>().CountAsync();
            }
            
            if (gameCanConnect)
            {
                gameCount = await _game.Set<account>().CountAsync();
            }
            
            return Ok(new { 
                authCanConnect, 
                gameCanConnect,
                authCount,
                gameCount
            });
        }

        [HttpGet]
        public async Task<IActionResult> RedisHealth()
        {
            try
            {
                var isConnected = _redis.IsConnected();
                var connectionStatus = _redis.GetConnectionStatus();
                
                // Redis 연결 테스트
                var testKey = "health_check";
                var testValue = DateTime.UtcNow.ToString();
                await _redis.SetAsync(testKey, testValue, TimeSpan.FromSeconds(10));
                var retrievedValue = await _redis.GetAsync(testKey);
                var testPassed = retrievedValue == testValue;
                
                return Ok(new { 
                    isConnected, 
                    connectionStatus, 
                    testPassed,
                    testValue,
                    retrievedValue
                });
            }
            catch (Exception ex)
            {
                return Ok(new { 
                    isConnected = false, 
                    connectionStatus = "Error", 
                    testPassed = false,
                    error = ex.Message
                });
            }
        }
    }
}
