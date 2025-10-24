using GatiToolkit.Shared.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GatiToolkit.Server.Controllers
{
    public partial class ScheduleController
    {
        [HttpGet("tasks")]
        public IActionResult GetTasks([FromQuery] string calendarID)
        {
            var calendar = GetScheduleCalendar(calendarID);
            if (calendar == null)
            {
                return NotFound();
            }
            return Ok(calendar.Tasks);
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
        public IActionResult AddTask([FromBody] CreateScheduleTaskRequest request)
        {
            var calendar = GetScheduleCalendar(request.CalendarID);
            if (calendar == null)
            {
                return NotFound();
            }

            List<string> newTasks = [];
            TimeSpan duration = TimeSpan.Zero;
            switch (request.RepeatType)
            {
                case CreateScheduleTaskRequest.RepeatTypes.None:
                    duration = TimeSpan.Zero;
                    break;

                case CreateScheduleTaskRequest.RepeatTypes.Daily:
                    duration = TimeSpan.FromDays(1);
                    break;

                case CreateScheduleTaskRequest.RepeatTypes.Weekly:
                    duration = TimeSpan.FromDays(7);
                    break;
            }

            for (int i = 0; i < request.RepeatCount; ++i)
            {
                newTasks.Add(calendar.AddTask(request.StartDate.Add(duration * i), request.EndDate.Add(duration * i)));
            }

            return Ok(newTasks);
        }

        [HttpDelete("task")]
        public IActionResult DeleteTask([FromBody] DeleteScheduleTaskRequest request)
        {
            var calendar = GetScheduleCalendar(request.CalendarID);
            if (calendar == null)
            {
                return NotFound();
            }

            calendar.RemoveTask(request.TaskID);
            return Ok();
        }

        [HttpPatch("task")]
        public IActionResult UpdateTask([FromBody] UpdateScheduleTaskRequest request)
        {
            var task = GetScheduleTask(request.CalendarID, request.TaskID);
            if (task == null)
            {
                return NotFound();
            }

            if (request.Description != task.Description)
            {
                task.ChangeDescription(request.Description);
            }

            if (request.Name != task.Name)
            {
                task.ChangeName(request.Name);
            }

            return Ok();
        }
    }
}
