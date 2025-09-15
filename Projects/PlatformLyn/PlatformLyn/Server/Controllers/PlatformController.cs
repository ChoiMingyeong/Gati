using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[action]")]
    public class PlatformController : ControllerBase
    {
        [HttpGet]
        public IActionResult Hello() => Ok(new { Message = "OK" });
    }
}
