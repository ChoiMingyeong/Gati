using GatiToolkit.Core.Schedule;
using Microsoft.AspNetCore.Mvc;

namespace GatiToolkit.Server.Controllers
{
    public partial class ScheduleController
    {
        [HttpGet("calendars")]
        public IActionResult GetCalendars()
        {
            return Ok(_scheduleCalendars);
        }

        [HttpGet("calendar")]
        public IActionResult GetCalendar([FromQuery] string id)
        {
            var calendar = GetScheduleCalendar(id);
            if (calendar == null)
            {
                return NotFound();
            }
            return Ok(calendar);
        }

        [HttpPatch("calendar/name")]
        public IActionResult ChangeCalendarName([FromQuery] string id, [FromBody] string name)
        {
            var calendar = GetScheduleCalendar(id);
            if (calendar == null)
            {
                return NotFound();
            }
            calendar.ChangeName(name);
            return Ok();
        }

        [HttpPut("calendar")]
        public IActionResult CreateCalendar()
        {
            var newCalendar = new ScheduleCalendar();
            _scheduleCalendars.Add(newCalendar);
            return Ok(newCalendar);
        }

        [HttpDelete("calendar")]
        public IActionResult DeleteCalendar([FromQuery] string id)
        {
            var calendar = GetScheduleCalendar(id);
            if (calendar == null)
            {
                return NotFound();
            }
            _scheduleCalendars.Remove(calendar);
            return Ok();
        }

    }
}
