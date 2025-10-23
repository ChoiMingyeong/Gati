using GatiToolkit.Core.Schedule;
using Microsoft.AspNetCore.Mvc;

namespace GatiToolkit.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class ScheduleController : ControllerBase
    {
        private static readonly List<ScheduleCalendar> _scheduleCalendars = [];

        private ScheduleCalendar? GetScheduleCalendar(string id) => _scheduleCalendars.SingleOrDefault(p => p.ID == id);
        private ScheduleTask? GetScheduleTask(string calendarID, string taskID) => GetScheduleCalendar(calendarID)?.GetTask(taskID);

        private readonly ILogger<ScheduleController> _logger;

        public ScheduleController(ILogger<ScheduleController> logger)
        {
            _logger = logger;
        }
    }
}
