using Microsoft.AspNetCore.Mvc;

namespace GatiToolkit.Server.Controllers
{
    public partial class ScheduleController
    {
        [HttpGet("tasks")]
        public IActionResult GetTasks([FromQuery] string calendarID, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var calendar = GetScheduleCalendar(calendarID);
            if (calendar == null)
            {
                return NotFound();
            }
            var tasks = calendar.GetTasks(startDate, endDate);
            return Ok(tasks);
        }

        [HttpGet("task")]
        public IActionResult GetTask([FromQuery] string calendarID, [FromQuery] string taskID)
        {
            var task = GetScheduleTask(calendarID, taskID);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [HttpPut("task")]
        public IActionResult AddTask([FromQuery] string calendarID, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var calendar = GetScheduleCalendar(calendarID);
            if (calendar == null)
            {
                return NotFound();
            }
            return Ok(calendar.AddTask(startDate, endDate));
        }

        [HttpDelete("task")]
        public IActionResult DeleteTask([FromQuery] string calendarID, [FromQuery] string taskID)
        {
            var calendar = GetScheduleCalendar(calendarID);
            if (calendar == null)
            {
                return NotFound();
            }

            calendar.RemoveTask(taskID);
            return Ok();
        }

        [HttpPatch("task/name")]
        public IActionResult ChangeTaskName([FromQuery] string calendarID, [FromQuery] string taskID, [FromBody] string name)
        {
            var task = GetScheduleTask(calendarID, taskID);
            if (task == null)
            {
                return NotFound();
            }
            task.ChangeName(name);
            return Ok();
        }
    }
}
