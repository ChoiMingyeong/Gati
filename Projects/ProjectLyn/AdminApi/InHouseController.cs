using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdminApi.AdminRequest;
using ServerLib;
using ServerLib.Model;

namespace AdminApi.Controllers
{
    [ApiController]
    [Route("InHouse/[action]")]
    public class InHouseController : ControllerBase
    {
        private readonly AuthRepository _authRepository;

        public InHouseController(AuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        [Authorize]
        [HttpGet]
        public IActionResult Validate() => Ok(new { valid = true });

        [HttpPost]
        public async Task<IActionResult> Login(AdminRequest.LoginData req)
        {
            // DB에서 사용자 검증
            var auth = await _authRepository.GetAuthByEmail(req.Email);
            if (auth == null || req.Password != "123") // 실제로는 해시된 비밀번호 비교
            {
                return Unauthorized(new { success = false, message = "invalid credentials" });
            }

            // TODO : UserEmail이 아닌 다른 string id 값 부여해서 사용하도록 수정
            // 토큰 발급
            var token = ServerLib.ServerUtil.Instance.Token.CreateJwtToken(auth.UserEmail);

            return Ok(new { success = true, token = token, user = new { auth.UserId, auth.UserEmail } });
        }

        //-----------------------------------------------------------------------------------------

        [HttpGet]
        public IActionResult GetData()
        {
            return Ok(new { message = "success", data = "test" });
        }

        [HttpPost]
        public IActionResult PostData()
        {
            return Ok(new { message = "success", data = "test" });
        }

        [Authorize]
        [HttpPost] //https://localhost:8001/InHouse/PostData2
        public IActionResult PostData2(SendData data)
        {
            var resData = new List<string>();
            if (data.Data == 0)
            {
                resData.Add("PostData2 data is 0");
            }
            else
            {
                resData.Add("PostData2 data is not 0");
            }

            var result = new AdminResponse.GetData()
            {
                Result = new AdminResponse.Result() { Code = 0, Message = string.Empty },
                Data = resData,
            };

            return new JsonResult(result);
        }

        [HttpPost] //https://localhost:8001/InHouse/PostData3?data=0
        public IActionResult PostData3(int data)
        {
            var resData = new List<string>();
            if (data == 0)
            {
                resData.Add("PostData3 data is 0");
            }
            else
            {
                resData.Add("PostData3 data is not 0");
            }

            var result = new AdminResponse.GetData()
            {
                Result = new AdminResponse.Result() { Code = 0, Message = string.Empty },
                Data = resData,
            };

            return new JsonResult(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAuth()
        {
            // 실제로는 페이징 처리 필요
            return Ok(new { message = "Get all auth endpoint - implement with pagination" });
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuth([FromBody] auth auth)
        {
            try
            {
                var createdAuth = await _authRepository.CreateAuthAsync(auth);
                return Ok(new { success = true, auth = createdAuth });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
