using Microsoft.AspNetCore.Mvc;

namespace APIServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        public ProjectController()
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetProjectsAsync()
        {
            // Simulate fetching projects from a database or service
            var projects = new List<Common.Models.Project>
            {
            };
            return Ok(projects);
        }
    }
}
