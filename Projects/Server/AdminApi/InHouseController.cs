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

        [HttpPost]
        public IActionResult Login(AdminRequest.LoginData req)
        {
            // DB 검증
            if (req.Password == "123")
            {
                return Ok(new { success = true, token = "123token" });
            }
            return Unauthorized(new { success = false });
        }

        [HttpGet]
        public IActionResult Validate([FromQuery] string token)
        {
            // JWT 검증
            if (token == "123token")
                return Ok(new { valid = true});
            return Unauthorized(new { valid = false });
        }
    }
}
