using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdminApi.AdminRequest;

namespace AdminApi.Controllers
{
    [ApiController]
    [Route("InHouse/[action]")]
    public class InHouseController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult Validate() => Ok(new { valid = true });

        [HttpPost]
        public IActionResult Login(AdminRequest.LoginData req)
        {
            // 사용자 검증 (DB 검증으로 변경예정)
            if (req.Email == null || req.Password != "123")
            {
                return Unauthorized(new { success = false, message = "invalid credentials" });
            }

            var username = "username";

            // 토큰 발급
            var token = ServerLib.ServerUtil.Instance.Token.CreateJwtToken(username);

            return Ok(new { success = true, token = token });

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
    }
}
